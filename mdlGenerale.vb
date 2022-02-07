Module mdlGenerale
	Public TipoDB As String = ""

	Public Function RitornaDataOra() As String
		Return Now.Year & "/" & Format(Now.Month, "00") & "/" & Format(Now.Day, "00") & " " & Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00") & " -> "
	End Function
End Module
