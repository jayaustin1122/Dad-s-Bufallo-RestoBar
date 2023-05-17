Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class Suppliers
    Private currentTime As DateTime ' declare a variable to store the current time
    ' Create a CultureInfo object that represents the Philippines
    Dim phCulture As New CultureInfo("fil-PH")
    Private Sub Suppliers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        addFoods.Enabled = False
        DisplayNameEmployee()

        Label7.Text = Date.Now.ToString("ddd, dd-MM-yyyy")
        Timer1.Interval = 1000 ' set the interval of the Timer control to 1000 milliseconds (1 second)
        Timer1.Start() ' start the Timer control
        Load_All()

    End Sub
    Sub Load_All()
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT Su_fullName, Su_address, Su_contact, Su_company FROM suppliers", conn)
            dr = cmd.ExecuteReader
            ' Use a loop to read all rows from the data reader
            While dr.Read()
                ' Use parentheses to access values from the data reader
                ' Use the Cells property to set the values in the DataGridView
                DataGridView1.Rows.Add(
                    DataGridView1.Rows.Count + 1,
                    dr("Su_fullName"),
                    dr("Su_address"),
                    dr("Su_contact"),
                    dr("Su_company")
                    )


            End While
        Catch ex As Exception
            ' Display the error message in a message box
            MsgBox(ex.Message)
        Finally
            ' Use a Finally block to ensure that the connection is always closed
            conn.Close()
        End Try
    End Sub

    Sub DisplayNameEmployee()
        Dim connection As New MySqlConnection("server=localhost;user=root;password=;port=3306;database=dad's buffalo restobar")

        Dim commandText As String = "SELECT * FROM employee"
        Dim command As New MySqlCommand(commandText, connection)

        Dim adapter As New MySqlDataAdapter(command)
        Dim dataTable As New DataTable()
        adapter.Fill(dataTable)

        txtEmployeeAssists.DataSource = dataTable
        txtEmployeeAssists.DisplayMember = "Emp_fullName"
        txtEmployeeAssists.ValueMember = "Emp_id"

    End Sub
   


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        currentTime = DateTime.Now ' update the value of the currentTime variable with the current time
        Time.Text = currentTime.ToString("hh:mm:ss tt") ' update the text of the Label control with the value of the currentTime variable
        ' Get_grandTotal()
        'Get_pricedata()
    End Sub

    Private Sub supplierContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles supplierContact.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub proPrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles proPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub proQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles proQty.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub proName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles proName.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar = "") Then
            e.Handled = True
        End If
    End Sub

    Private Sub supplierFname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles supplierFname.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsControl(e.KeyChar) Or e.KeyChar = "") Then
            e.Handled = True
        End If
    End Sub
    Sub saveSupplier()

        Dim connString As String = "server=localhost;user=root;password=;database=dad's buffalo restobar"

        Using conn As New MySqlConnection(connString)
            Using cmd As New MySqlCommand()
                cmd.Connection = conn

                Try
                    cmd.CommandText = "INSERT INTO suppliers(Su_fullName,Su_address,Su_contact,Su_company) VALUES(@fullname, @address, @contact,@company)"
                    cmd.Parameters.AddWithValue("@fullname", supplierFname.Text)
                    cmd.Parameters.AddWithValue("@address", supplierAddress.Text)
                    cmd.Parameters.AddWithValue("@contact", supplierContact.Text)
                    cmd.Parameters.AddWithValue("@company", supplierCompany.Text)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Supplier successfully Inserted and Registered!")
                        saveProducts()

                    Catch ex As MySqlException
                        MessageBox.Show("Failed to insert data: " + ex.Message)
                    End Try
                Catch ex As Exception
                    MsgBox("Error: " + ex.Message)
                End Try

            End Using
        End Using

    End Sub

    Sub saveProducts()

        Try

            Dim cmd As New MySqlCommand("INSERT INTO `drinks`(`Dr_foodcode`, `Dr_name`, `Dr_price`,`Dr_img`,`Dr_qty`) VALUES (@foodcode,@foodname,@price,@img,@qty)", conn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@foodcode", txt_transno.Text)
            cmd.Parameters.AddWithValue("@foodname", proName.Text)
            cmd.Parameters.AddWithValue("@price", proPrice.Text)
            cmd.Parameters.AddWithValue("@qty", proQty.Text)
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
                MsgBox("New Product Save Successfully !", vbInformation, "Success!")
            Else
                MsgBox("Warning : Drink Save Failed !", vbCritical, "May Mali")
            End If


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        clear()



    End Sub

    Private Sub clear()
        supplierFname.Clear()
        supplierAddress.Clear()
        supplierContact.Clear()
        supplierCompany.Clear()
        proName.Clear()
        proPrice.Clear()
        proQty.Clear()

    End Sub
    Function getSupplierId() As Integer
        Dim finalId As Integer = 0
        conn.Open()
        Try
            cmd.CommandText = "SELECT MAX(Su_id) FROM suppliers"
            Dim result As Object = cmd.ExecuteScalar()

            If result Is DBNull.Value Then
                finalId = 1
            Else
                finalId = CInt(result) + 1
            End If

            conn.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        Return finalId
    End Function

    Private Sub addDrinks_Click(sender As Object, e As EventArgs) Handles addDrinks.Click
        Dim nextId As Integer = getSupplierId()

        If supplierFname.Text = "" Or supplierAddress.Text = "" Or supplierContact.Text = "" Or supplierCompany.Text = "" Or proName.Text = "" Or proPrice.Text = "" Or proQty.Text = "" Then
            MsgBox("Information Missing")
            Return
        Else
            Try
                conn.Open()

                ' Specify the correct parameters in the query
                Dim cmd As New MySqlCommand("INSERT INTO `supplies`(`Su_id`, `Dr_id`, `Sups_date`, `Sups_qty`, `Sups_cost`) VALUES (@suId, @drId, @date, @qty, @cost)", conn)
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@suId", nextId)
                cmd.Parameters.AddWithValue("@drId", txt_transno.Text)
                cmd.Parameters.AddWithValue("@date", Date.Now.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@qty", proQty.Text)
                cmd.Parameters.AddWithValue("@cost", proPrice.Text)

                i = cmd.ExecuteNonQuery()

                If i > 0 Then
                    ' Specify the correct parameters in the query
                    cmd = New MySqlCommand("INSERT INTO `assists`(`Emp_id`, `Su_id`, `As_date`) VALUES (@empId, @suId, @asDate)", conn)
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@empId", txtEmployeeAssists.SelectedValue)
                    cmd.Parameters.AddWithValue("@suId", nextId)
                    cmd.Parameters.AddWithValue("@asDate", Date.Now.ToString("yyyy-MM-dd"))
                    cmd.ExecuteNonQuery()
                    saveSupplier()
                    clear()
                Else
                    ' Insert failed
                    MsgBox("Insert failed")
                End If
            Catch ex As Exception
                ' Exception occurred
                MsgBox("Error: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DataGridView1.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT Su_fullName, Su_address, Su_contact, Su_company FROM suppliers WHERE Su_fullName LIKE @search OR Su_address LIKE @search OR Su_contact LIKE @search OR Su_company LIKE @search  ", conn)
            ' Fixed syntax error in WHERE clause
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView1.Rows.Add(
                   DataGridView1.Rows.Count + 1,
                   dr("Su_fullName"),
                   dr("Su_address"),
                   dr("Su_contact"),
                   dr("Su_company")
                   )

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim pop As OpenFileDialog = New OpenFileDialog
        If pop.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            PictureBox3.Image = Image.FromFile(pop.FileName)
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT * FROM `suppliers` WHERE `Su_fullName`=@name", conn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@name", supplierFname.Text)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim name As String = dr.Item("Su_fullName")
                Dim address As String = dr.Item("Su_address")
                Dim contact As String = dr.Item("Su_contact")
                Dim company As String = dr.Item("Su_company")

                supplierFname.Text = name
                supplierAddress.Text = address
                supplierContact.Text = contact
                supplierCompany.Text = company
              
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub
End Class