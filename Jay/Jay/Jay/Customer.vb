Imports System.IO
Imports MySql.Data.MySqlClient
Public Class Customer

    Private Sub Customer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        Load_All()
    End Sub


    Sub Load_All()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT Co_fullName, Co_address, Co_email, Co_age, Co_contact FROM costumer", conn)
            dr = cmd.ExecuteReader
            ' Use a loop to read all rows from the data reader
            While dr.Read()
                ' Use parentheses to access values from the data reader
                ' Use the Cells property to set the values in the DataGridView
                DataGridView1.Rows.Add(
                    DataGridView1.Rows.Count + 1,
                    dr("Co_fullName"),
                    dr("Co_address"),
                    dr("Co_email"),
                    dr("Co_age"),
                    dr("Co_contact"))
                   

            End While
        Catch ex As Exception
            ' Display the error message in a message box
            MsgBox(ex.Message)
        Finally
            ' Use a Finally block to ensure that the connection is always closed
            conn.Close()
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT Co_fullName, Co_address, Co_email, Co_age, Co_contact FROM costumer WHERE Co_fullName LIKE @search OR Co_address LIKE @search OR Co_email LIKE @search OR Co_age LIKE @search OR Co_contact LIKE @search ", conn)
            ' Fixed syntax error in WHERE clause
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(
                    DataGridView1.Rows.Count + 1,
                    dr("Co_fullName"),
                    dr("Co_address"),
                    dr("Co_email"),
                    dr("Co_age"),
                    dr("Co_contact"))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

    End Sub

End Class