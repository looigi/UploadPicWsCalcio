Module mdlGenerale
	Public TipoDB As String = ""
	Public Const ErroreConnessioneNonValida As String = "ERRORE: Stringa di connessione non valida"
	Public Const ErroreConnessioneDBNonValida As String = "ERRORE: Connessione al db non valida"

	Public Function RitornaDataOra() As String
		Return Now.Year & "/" & Format(Now.Month, "00") & "/" & Format(Now.Day, "00") & " " & Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00") & " -> "
	End Function

	Public Function LeggeImpostazioniDiBase(Percorso As String, Squadra As String) As String
		Dim Connessione As String = ""

		' Impostazioni di base
		Dim ListaConnessioni As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings

		If ListaConnessioni.Count <> 0 Then
			' Get the collection elements. 
			For Each Connessioni As ConnectionStringSettings In ListaConnessioni
				Dim Nome As String = Connessioni.Name
				Dim Provider As String = Connessioni.ProviderName
				Dim connectionString As String = Connessioni.ConnectionString

				If TipoDB = "SQLSERVER" Then
					If Nome = "SQLConnectionStringLOCALESS" Then
						Connessione = "Provider=" & Provider & ";" & connectionString
						Connessione = Replace(Connessione, "*^*^*", Percorso & "\")
						If Squadra <> "" Then
							If Squadra = "DBVUOTO" Then
								Connessione = Connessione.Replace("***NOME_DB***", "DBVuoto")
							Else
								Connessione = Connessione.Replace("***NOME_DB***", Squadra)
							End If
						Else
							Connessione = Connessione.Replace("***NOME_DB***", "Generale")
						End If
						Exit For
					End If
				Else
					If Nome = "SQLConnectionStringLOCALEMD" Then
						Connessione = connectionString
						Connessione = Replace(Connessione, "*^*^*", Percorso & "\")
						If Squadra <> "" Then
							If Squadra = "DBVUOTO" Then
								Connessione = Connessione.Replace("***NOME_DB***", "DBVuoto")
							Else
								Connessione = Connessione.Replace("***NOME_DB***", Squadra)
							End If
						Else
							Connessione = Connessione.Replace("***NOME_DB***", "Generale")
						End If
						Exit For
					End If
				End If
			Next
		End If

		Return Connessione
	End Function
End Module
