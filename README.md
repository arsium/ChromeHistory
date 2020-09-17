# ChromeHistory
This library can get many information about chrome history !
This dll is my own researches about getting chrome history logs , you can add by yourself others chromium-based web browsers !

How to use it ?

VBNET : 

```Visual Basic .NET
        For Each h In History.History_Log.History_Grab()
            RichTextBox1.AppendText(h.browser & "  " & h.Title & "  " & h.visit_count & "  " & h.last_visit_Time & "   " & h.URL & vbNewLine & vbNewLine)
        Next

        For Each h In History.History_Log.Downloads_Grab
            RichTextBox2.AppendText(h.browser & "  " & h.target_path & "  " & h.current_path & "  " & h.site_url & "   " & h.end_time & vbNewLine & vbNewLine)
        Next

        For Each h In History.History_Log.SearchTerms_Grab
            RichTextBox3.AppendText(h.keyword_id & "  " & h.term & "  " & h.url_id & "  " & h.normalized_term & vbNewLine & vbNewLine)
        Next
    
```
