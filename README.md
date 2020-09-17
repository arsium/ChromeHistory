# ChromeHistory
This library can get many information about chrome history !
This dll is my own researches about getting chrome history logs , you can add by yourself others chromium-based web browsers !

How to use it ?

VBNET : 

For Each h In History.History_Log.Accounts()
            RichTextBox1.AppendText(h.Title & "  " & h.visit_count & "  " & h.last_visit_Time & "   " & h.URL & vbNewLine & vbNewLine)
Next

