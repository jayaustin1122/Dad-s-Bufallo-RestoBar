Public Class Form1

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

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

  
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With Drinks
            .TopLevel = False
            Panel5.Controls.Add(Drinks)
            .BringToFront()
            .Show()
        End With
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        With Transaction
            .TopLevel = False
            Panel5.Controls.Add(Transaction)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MsgBox("Are you sure Exit !", vbQuestion + vbYesNo) = vbYes Then
            End
        Else
            Return
        End If
        End

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        With Customer

            .TopLevel = False
            Panel5.Controls.Add(Customer)
            .BringToFront()
            .Show()
        End With
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        With Suppliers


            .TopLevel = False
            Panel5.Controls.Add(Suppliers)
            .BringToFront()
            .Show()
        End With
    End Sub


End Class