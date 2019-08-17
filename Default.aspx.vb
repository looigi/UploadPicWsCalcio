Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class _Default
    Inherits System.Web.UI.Page

    Private Sub form1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Init
        Dim vTipologia As String = ""
        Dim vNomeFile As String = ""
		Dim vCartella As String = ""
		Dim Arrotonda As String = ""

		Dim FilePathImmagine As String = "C:\inetpub\wwwroot\CVCalcio\App_Themes\Standard\Images\"

        If Not String.IsNullOrEmpty(Request.Form("tipologia")) Then
            vTipologia = Request.Form("tipologia")
        End If
        If Not String.IsNullOrEmpty(Request.Form("nomefile")) Then
            vNomeFile = Request.Form("nomefile")
        End If
        If Not String.IsNullOrEmpty(Request.Form("cartella")) Then
            vCartella = Request.Form("cartella")
        End If
		If Not String.IsNullOrEmpty(Request.Form("arrotonda")) Then
			Arrotonda = Request.Form("arrotonda")
		End If

		Dim MyFileCollection As HttpFileCollection = Request.Files

        If MyFileCollection.Count > 0 Then
            Dim NomeFile As String
            Dim Contatore As Integer = 0
            Dim Altro As String = ""

            If vCartella <> "" Then
                Altro = vCartella & "\"
            End If

            NomeFile = FilePathImmagine & vTipologia & "\" & Altro & vNomeFile
            NomeFile = NomeFile.Replace("\Allenatori\Allenatori\", "\Allenatori\")
            NomeFile = NomeFile.Replace("\Arbitri\Arbitri\", "\Arbitri\")
            NomeFile = NomeFile.Replace("\Avversari\Avversari\", "\Avversari\")
            NomeFile = NomeFile.Replace("\Categorie\Categorie\", "\Categorie\")
            NomeFile = NomeFile.Replace("\Dirigenti\Dirigenti\", "\Dirigenti\")
            NomeFile = NomeFile.Replace("\Giocatori\Giocatori\", "\Giocatori\")

            Dim gf As New GestioneFilesDirectory
            gf.CreaDirectoryDaPercorso(gf.TornaNomeDirectoryDaPath(NomeFile) & "\")

			MyFileCollection(0).SaveAs(NomeFile)

			If Arrotonda = "SI" Then
				Dim gi As New GestioneImmagini
				Dim pathFile As String = gf.TornaNomeDirectoryDaPath(NomeFile)
				If Not pathFile.EndsWith("\") Then
					pathFile &= "\"
				End If
				NomeFile = gf.TornaNomeFileDaPath(NomeFile)

				'gf.ApreFileDiTestoPerScrittura(pathFile & "log.txt")
				'gf.ScriveTestoSuFileAperto(pathFile)
				'gf.ScriveTestoSuFileAperto(NomeFile)
				'gf.ScriveTestoSuFileAperto("Ridimensiona")

				gi.Ridimensiona(pathFile & NomeFile, pathFile & "Appoggio.png", 255, 255)

				'gf.ScriveTestoSuFileAperto("Elimina file")
				gf.EliminaFileFisico(pathFile & NomeFile)

				'gf.ScriveTestoSuFileAperto("Arrotonda")
				gi.RidimensionaEArrotondaIcona(pathFile & "Appoggio.png", pathFile & NomeFile)

				'gf.ScriveTestoSuFileAperto("Elimina file di appoggio")
				gf.EliminaFileFisico(pathFile & "Appoggio.png")

				'gf.ChiudeFileDiTestoDopoScrittura()

				gi = Nothing
			End If
			gf = Nothing

		End If
    End Sub

	Protected Sub btnCOnverte_Click(sender As Object, e As EventArgs) Handles btnCOnverte.Click
		Dim pathLettura As String = System.Configuration.ConfigurationManager.AppSettings("pathImm").ToString()
		If Not pathLettura.EndsWith("\") Then
			pathLettura &= "\"
		End If
		Dim pathScrittura As String = pathLettura & "Output\"
		Dim gf As New GestioneFilesDirectory
		Dim gi As New GestioneImmagini
		gf.CreaDirectoryDaPercorso(pathScrittura)
		gf.ScansionaDirectorySingola(pathLettura)
		Dim filetti() As String = gf.RitornaFilesRilevati
		Dim qfiletti As Integer = gf.RitornaQuantiFilesRilevati
		For i As Integer = 1 To qfiletti
			Dim nomeOutput As String = filetti(i)
			nomeOutput = gf.TornaNomeFileDaPath(nomeOutput)
			nomeOutput = pathScrittura & nomeOutput
			gf.EliminaFileFisico(Server.MapPath(".") & "\Appoggio.png")
			gi.Ridimensiona(filetti(i), Server.MapPath(".") & "\Appoggio.png", 255, 255)
			gi.RidimensionaEArrotondaIcona(Server.MapPath(".") & "\Appoggio.png", nomeOutput)
		Next
		gf = Nothing
	End Sub
End Class