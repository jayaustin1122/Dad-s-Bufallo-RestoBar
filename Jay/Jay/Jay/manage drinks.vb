
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class manage_foods
    Dim phCulture As New CultureInfo("fil-PH")
    Private Sub manage_foods_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        DataGridView1.RowTemplate.Height = 30
        DataGridView1.Columns("Column4").DefaultCellStyle.Format = "C"
        txt_foodcode.Enabled = False
        auto_foodcode()
        Load_fooddata()
    End Sub

    Sub Load_fooddata()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Dr_foodcode`, `Dr_name`, `Dr_price`, `Dr_qty` FROM `drinks`", conn)
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(DataGridView1.Rows.Count + 1, dr.Item("Dr_foodcode"), dr.Item("Dr_name"), dr.Item("Dr_price"), dr.Item("Dr_qty"))

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim pop As OpenFileDialog = New OpenFileDialog
        If pop.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            PictureBox3.Image = Image.FromFile(pop.FileName)
        End If
    End Sub

    Sub auto_foodcode()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT * FROM `drinks` order by Dr_id desc", conn)
            dr = cmd.ExecuteReader
            dr.Read()
            If dr.HasRows = True Then
                txt_foodcode.Text = dr.Item("Dr_foodcode").ToString + 1

            Else
                txt_foodcode.Text = Date.Now.ToString("yyyyMM") & "001"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub
    Sub clear()
        txt_foodname.Clear()
        txt_price.Clear()
        PictureBox3.Image = Nothing
        textBoxQuantity.Clear()
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("INSERT INTO `drinks`(`Dr_foodcode`, `Dr_name`, `Dr_price`, `Dr_img`, `Dr_qty`) VALUES (@foodcode,@foodname,@price,@img,@qty)", conn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@foodcode", txt_foodcode.Text)
            cmd.Parameters.AddWithValue("@foodname", txt_foodname.Text)
            cmd.Parameters.AddWithValue("@price", CDec(txt_price.Text))
            cmd.Parameters.AddWithValue("@qty", textBoxQuantity.Text)
            Dim FileSize As New UInt32
            Dim mstream As New System.IO.MemoryStream
            PictureBox3.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim picture() As Byte = mstream.GetBuffer
            FileSize = mstream.Length
            mstream.Close()
            cmd.Parameters.AddWithValue("@img", picture)

            Dim i As Integer
            i = cmd.ExecuteNonQuery
            If i > 0 Then
                MsgBox("New Drink Save Successfully !", vbInformation, "meoww")
            Else
                MsgBox("Warning : Drink Save Failed !", vbCritical, "sampleselfksjdfh")
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        clear()
        auto_foodcode()
        Load_fooddata()
        Drinks.Load_Foods()
    End Sub

    Private Sub btn_edit_Click(sender As Object, e As EventArgs) Handles btn_edit.Click
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("UPDATE `drinks` SET `Dr_name`=@foodname,`Dr_price`=@price,`Dr_img`=@img,`Dr_qty`=@qty WHERE `Dr_foodcode`=@foodcode", conn)
            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@foodname", txt_foodname.Text)
            cmd.Parameters.AddWithValue("@price", CDec(txt_price.Text))
            Dim FileSize As New UInt32
            Dim mstream As New System.IO.MemoryStream
            PictureBox3.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim picture() As Byte = mstream.GetBuffer
            FileSize = mstream.Length
            mstream.Close()
            cmd.Parameters.AddWithValue("@img", picture)
            cmd.Parameters.AddWithValue("@foodcode", txt_foodcode.Text)
            cmd.Parameters.AddWithValue("@qty", textBoxQuantity.Text)
            Dim i As Integer
            i = cmd.ExecuteNonQuery
            If i > 0 Then
                MsgBox("Drink Edit Successfully !", vbInformation, "asd")
            Else
                MsgBox("Warning : Food Edit Failed !", vbCritical, "dasdsd")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        clear()
        auto_foodcode()
        Load_fooddata()
        Drinks.Load_Foods()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        If MsgBox("Are you Sure Delete this Drink Product !", vbQuestion + vbYesNo, "Dad's") = vbYes Then
            Try
                conn.Open()
                cmd = New MySqlCommand("DELETE FROM `drinks` WHERE `Dr_foodcode`=@foodcode", conn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@foodcode", txt_foodcode.Text)
                Dim i As Integer
                i = cmd.ExecuteNonQuery
                If i > 0 Then
                    MsgBox("Drink Delete Successfully !", vbInformation, "Dad's")
                Else
                    MsgBox("Warning : Food Delete Failed !", vbCritical, "Dad's")
                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            conn.Close()
            clear()
            clear()
            auto_foodcode()
            Load_fooddata()
            Drinks.Load_Foods()
        Else
            Return

        End If
    End Sub


    Private Sub btn_find_Click(sender As Object, e As EventArgs) Handles btn_find.Click
        clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT * FROM `drinks` WHERE `Dr_name`=@Drinkcode", conn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@Drinkcode", txt_found.Text)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim foodcode As String = dr.Item("Dr_foodcode")
                Dim foodname As String = dr.Item("Dr_name")
                Dim price As Decimal = dr.Item("Dr_price")
                Dim qty As Integer = dr.Item("Dr_qty")
                txt_foodcode.Text = foodcode
                txt_foodname.Text = foodname
                txt_price.Text = price
                textBoxQuantity.Text = qty
                Dim bytes As [Byte]() = dr.Item("Dr_img")
                Dim ms As New MemoryStream(bytes)
                PictureBox3.Image = Image.FromStream(ms)

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub

    Private Sub btn_close_Click(sender As Object, e As EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub



    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Dr_foodcode`, `Dr_name`, `Dr_price`, `Dr_qty` FROM `drinks` WHERE Dr_foodcode LIKE @search OR Dr_name LIKE @search OR Dr_price LIKE @search OR Dr_qty LIKE @search", conn)
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(DataGridView1.Rows.Count + 1, dr.Item("Dr_foodcode"), dr.Item("Dr_name"), dr.Item("Dr_price"), dr.Item("Dr_qty"))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub txt_foodname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_foodname.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar = "") Then
            e.Handled = True
        End If
    End Sub

    Private Sub txt_price_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_price.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub textBoxQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles textBoxQuantity.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class