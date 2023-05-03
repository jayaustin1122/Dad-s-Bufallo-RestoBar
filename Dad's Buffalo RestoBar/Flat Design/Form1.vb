Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        With Form2
            .TopLevel = False
            Panel5.Controls.Add(Form2)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        With Employee
            .TopLevel = False
            Panel5.Controls.Add(Employee)
            .BringToFront()
            .Show()
        End With
    End Sub

End Class
