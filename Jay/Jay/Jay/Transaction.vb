Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class Transaction
    Dim phCulture As New CultureInfo("fil-PH")
    Private Sub Transaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        Load_All()
        Load_All2()
        DataGridView1.Columns("Column7").DefaultCellStyle.Format = "C"
        DataGridView2.Columns("supID").DefaultCellStyle.Format = "C"
        DataGridView1.Columns("Column4").DefaultCellStyle.Format = ""
        DataGridView2.Columns("foodID").DefaultCellStyle.Format = ""
   
        ' Set the custom date format string that displays only the date component
        Dim dateFormatString As String = "yyyy-MM-dd"

        ' Set the format for the column
        DataGridView1.Columns("Column3").DefaultCellStyle.Format = dateFormatString
        DataGridView2.Columns("dateColumn").DefaultCellStyle.Format = dateFormatString


    End Sub

    Sub Load_All()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT`Co_id`, `Fo_id`, `Dr_id`, `Or_qty`, `Or_cost`, `Or_transNo`, `Or_date` FROM `ordered`", conn)
            dr = cmd.ExecuteReader
            ' Use a loop to read all rows from the data reader
            While dr.Read()
                ' Use parentheses to access values from the data reader
                ' Use the Cells property to set the values in the DataGridView
                DataGridView1.Rows.Add(
                    DataGridView1.Rows.Count + 1,
                    dr("Or_transNo"),
                    dr("Or_date"),
                    dr("Co_id"),
                    dr("Fo_id"),
                    dr("Dr_id"),
                    dr("Or_cost"))
            End While
        Catch ex As Exception
            ' Display the error message in a message box
            MsgBox(ex.Message)
        Finally
            ' Use a Finally block to ensure that the connection is always closed
            conn.Close()
        End Try
    End Sub
    Sub Load_All2()
        DataGridView2.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Su_id`, `Dr_id`, `Fo_id`, `Sups_date`, `Sups_qty`, `Sups_cost` FROM `supplies`", conn)
            dr = cmd.ExecuteReader
            ' Use a loop to read all rows from the data reader
            While dr.Read()
                ' Use parentheses to access values from the data reader
                ' Use the Cells property to set the values in the DataGridView
                DataGridView2.Rows.Add(
                       DataGridView2.Rows.Count + 1,
                       dr("Su_id"),
                       dr("Dr_id"),
                       dr("Fo_id"),
                       dr("Sups_date"),
                       dr("Sups_qty"),
                       dr("Sups_cost"))
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
            cmd = New MySqlCommand("SELECT `Co_id`, `Fo_id`, `Dr_id`, `Or_qty`, `Or_cost`, `Or_transNo`, `Or_date` FROM `ordered` WHERE Co_id LIKE @search OR Fo_id LIKE @search OR Dr_id LIKE @search OR Or_qty LIKE @search OR Or_cost LIKE @search OR Or_transNo LIKE @search OR Or_date LIKE @search", conn)
            ' Fixed syntax error in WHERE clause
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(
                    DataGridView1.Rows.Count + 1,
                    dr("Or_transNo"),
                    dr("Or_date"),
                    dr("Co_id"),
                    dr("Fo_id"),
                    dr("Dr_id"),
                    dr("Or_cost"))

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

    End Sub


    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        DataGridView2.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Su_id`, `Dr_id`, `Fo_id`, `Sups_date`, `Sups_qty`, `Sups_cost` FROM `supplies` WHERE Su_id LIKE @search OR Dr_id LIKE @search OR Fo_id LIKE @search OR Sups_date LIKE @search OR Sups_qty LIKE @search OR Sups_cost LIKE @search", conn)
            ' Fixed syntax error in WHERE clause
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView2.Rows.Add(
                    DataGridView2.Rows.Count + 1,
                    dr("Su_id"),
                    dr("Dr_id"),
                    dr("Fo_id"),
                    dr("Sups_date"),
                    dr("Sups_qty"),
                    dr("Sups_cost"))

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

    End Sub

End Class