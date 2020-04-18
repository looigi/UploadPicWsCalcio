Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Security.Policy

Public Class _Default
	Inherits System.Web.UI.Page

	Private Sub form1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Init
		Dim vTipologia As String = ""
		Dim vNomeFile As String = ""
		Dim vCartella As String = ""
		Dim Arrotonda As String = ""
		Dim NomeSquadra As String = ""
		Dim ScriveLog As String = ""

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
		If Not String.IsNullOrEmpty(Request.Form("nomesquadra")) Then
			NomeSquadra = Request.Form("nomesquadra")
		End If
		If Not String.IsNullOrEmpty(Request.Form("scrivelog")) Then
			ScriveLog = Request.Form("scrivelog")
		End If

		Dim gf As New GestioneFilesDirectory
		Dim Percorsi As String = gf.LeggeFileIntero(Server.MapPath(".") & "\PercorsoImmagini.txt")
		Dim Paths() As String = Percorsi.Split(";")
		Dim FilePathImmagine As String = Paths(0)
		If Strings.Right(FilePathImmagine, 1) = "\" Then
			FilePathImmagine = Mid(FilePathImmagine, 1, FilePathImmagine.Length - 1)
		End If
		Dim FilePathLogs As String = Paths(1)
		If Strings.Right(FilePathLogs, 1) = "\" Then
			FilePathLogs = Mid(FilePathLogs, 1, FilePathLogs.Length - 1)
		End If

		If ScriveLog = "SI" Then
			gf.CreaDirectoryDaPercorso(FilePathLogs & "\")
			gf.ApreFileDiTestoPerScrittura(FilePathLogs & "\Log.txt")
		End If
		Dim Cosa As String = Now & Chr(13) & Chr(10)
		Cosa &= "---------------------------------" & Chr(13) & Chr(10)
		Cosa &= "Tipologia: " & vTipologia & Chr(13) & Chr(10)
		Cosa &= "Nome File: " & vNomeFile & Chr(13) & Chr(10)
		Cosa &= "Cartella: " & vCartella & Chr(13) & Chr(10)
		Cosa &= "Arrotonda: " & Arrotonda & Chr(13) & Chr(10)
		Cosa &= "Nome Squadra: " & NomeSquadra & Chr(13) & Chr(10)
		If ScriveLog = "SI" Then
			gf.ScriveTestoSuFileAperto(RitornaDataOra() & Cosa)
		End If
		' Response.End()

		Dim MyFileCollection As HttpFileCollection = Request.Files

		If MyFileCollection.Count > 0 Then
			' MyFileCollection(0).SaveAs("C:\BackupLog\imm.jpg")

			Dim NomeFile As String
			Dim Contatore As Integer = 0
			Dim Altro As String = ""

			If vCartella <> "" Then
				Altro = vCartella & "\"
			End If

			NomeFile = vTipologia & "\" & Altro & vNomeFile

			If NomeSquadra <> "" Then
				If Not NomeFile.StartsWith("\") Then
					NomeFile = "\" & NomeFile
				End If
				NomeFile = NomeSquadra & "\" & NomeFile
			End If

			NomeFile = FilePathImmagine & "\" & NomeFile
			NomeFile = NomeFile.Replace("\\", "\")

			NomeFile = NomeFile.Replace("\Allenatori\Allenatori\", "\Allenatori\")
			NomeFile = NomeFile.Replace("\Arbitri\Arbitri\", "\Arbitri\")
			NomeFile = NomeFile.Replace("\Avversari\Avversari\", "\Avversari\")
			NomeFile = NomeFile.Replace("\Categorie\Categorie\", "\Categorie\")
			NomeFile = NomeFile.Replace("\Dirigenti\Dirigenti\", "\Dirigenti\")
			NomeFile = NomeFile.Replace("\Giocatori\Giocatori\", "\Giocatori\")

			If ScriveLog = "SI" Then
				gf.ScriveTestoSuFileAperto(RitornaDataOra() & "Nome File Completo: " & NomeFile)
			End If

			' Dim gf As New GestioneFilesDirectory
			gf.CreaDirectoryDaPercorso(gf.TornaNomeDirectoryDaPath(NomeFile) & "\")

			MyFileCollection(0).SaveAs(NomeFile)
			If ScriveLog = "SI" Then
				gf.ScriveTestoSuFileAperto(RitornaDataOra() & "File salvato")
			End If

			If Arrotonda = "SI" Then
				If ScriveLog = "SI" Then
					gf.ScriveTestoSuFileAperto(RitornaDataOra() & "Arrotondo l'immagine. Inizio...")
				End If

				Dim gi As New GestioneImmagini
				Dim pathFile As String = gf.TornaNomeDirectoryDaPath(NomeFile)
				If Not pathFile.EndsWith("\") Then
					pathFile &= "\"
				End If
				NomeFile = gf.TornaNomeFileDaPath(NomeFile)

				'gf.ApreFileDiTestoPerScrittura(pathFile & "log.txt")
				'gf.ScriveTestoSuFileAperto(ritornadataora & pathFile)
				'gf.ScriveTestoSuFileAperto(ritornadataora & NomeFile)
				'gf.ScriveTestoSuFileAperto(ritornadataora & "Ridimensiona")

				gi.Ridimensiona(gf, ScriveLog, pathFile & NomeFile, pathFile & "Appoggio.png", 255, 255)
				If ScriveLog = "SI" Then
					gf.ScriveTestoSuFileAperto(RitornaDataOra() & "Arrotondo l'immagine. Ridimensionata...")
				End If

				'gf.ScriveTestoSuFileAperto(ritornadataora & "Elimina file")
				gf.EliminaFileFisico(pathFile & NomeFile)

				'gf.ScriveTestoSuFileAperto(ritornadataora & "Arrotonda")
				gi.RidimensionaEArrotondaIcona(gf, ScriveLog, pathFile & "Appoggio.png", pathFile & NomeFile)
				If ScriveLog = "SI" Then
					gf.ScriveTestoSuFileAperto(RitornaDataOra() & "Arrotondo l'immagine. Arrotondata...")
				End If

				'gf.ScriveTestoSuFileAperto(ritornadataora & "Elimina file di appoggio")
				gf.EliminaFileFisico(pathFile & "Appoggio.png")

				'gf.ChiudeFileDiTestoDopoScrittura()

				If ScriveLog = "SI" Then
					gf.ScriveTestoSuFileAperto(RitornaDataOra() & "Arrotondo l'immagine. Fine...")
				End If
				gi = Nothing
			End If
			If ScriveLog = "SI" Then
				gf.ScriveTestoSuFileAperto(RitornaDataOra() & "---------------------------------" & Chr(13) & Chr(10))
				gf.ChiudeFileDiTestoDopoScrittura()
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
			gi.Ridimensiona(gf, "NO", filetti(i), Server.MapPath(".") & "\Appoggio.png", 255, 255)
			gi.RidimensionaEArrotondaIcona(gf, "NO", Server.MapPath(".") & "\Appoggio.png", nomeOutput)
		Next
		gf = Nothing
	End Sub
End Class