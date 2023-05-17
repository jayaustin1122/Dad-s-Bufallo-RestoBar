Imports MySql.Data.MySqlClient
Public Class Employee

 
    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        Load_All()
        btn_edit.Enabled = False

    End Sub


    Sub Load_All()
        DataGridView2.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT Emp_fullName, Emp_email, Emp_address, Emp_contact, Emp_type, Emp_salary, Emp_age,Emp_gender FROM employee", conn)
            dr = cmd.ExecuteReader
            ' Use a loop to read all rows from the data reader
            While dr.Read()
                ' Use parentheses to access values from the data reader
                ' Use the Cells property to set the values in the DataGridView
                DataGridView2.Rows.Add(
                    DataGridView2.Rows.Count + 1,
                    dr("Emp_fullName"),
                    dr("Emp_email"),
                    dr("Emp_address"),
                    dr("Emp_contact"),
                    dr("Emp_type"),
                    dr("Emp_salary"),
                    dr("Emp_age"),
                      dr("Emp_gender")
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
    Public Sub ClearData()
        txtFullname.Clear()
        textEmail2.Clear()
        txtAddress.Clear()
        txtContact.Clear()
        txtType.SelectedIndex = -1
        txtSalary.Clear()
        txtAge.Clear()
        txtGender2.SelectedIndex = -1
        btn_edit.Enabled = False

    End Sub

   

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Dim connString As String = "server=localhost;user=root;password=;database=dad's buffalo restobar"
        Using conn As New MySqlConnection(connString)
            Using cmd As New MySqlCommand()
                cmd.Connection = conn
                If txtFullname.Text = "" Or textEmail2.Text = "" Or txtAddress.Text = "" Or txtContact.Text = "" Or txtType.Text = "" Or txtSalary.Text = "" Or txtAge.Text = "" Or txtGender2.Text = "" Then
                    MsgBox("Information Missing")
                    Return
                Else

                    Try
                        cmd.CommandText = "INSERT INTO `employee`(`Emp_fullName`, `Emp_email`, `Emp_address`, `Emp_contact`,`Emp_type`, `Emp_salary`, `Emp_age`, `Emp_gender`) VALUES(@fullname, @email, @address,@contact,@type, @salary,@age,@gender)"
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@fullname", txtFullname.Text)
                        cmd.Parameters.AddWithValue("@email", textEmail2.Text)
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text)
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text)
                        cmd.Parameters.AddWithValue("@type", txtType.Text)
                        cmd.Parameters.AddWithValue("@salary", txtSalary.Text)
                        cmd.Parameters.AddWithValue("@age", txtAge.Text)
                        cmd.Parameters.AddWithValue("@gender", txtGender2.Text)

                        Try
                            conn.Open()
                            cmd.ExecuteNonQuery()
                            MessageBox.Show("New Employee successfully Inserted!")
                            ClearData()
                        Catch ex As MySqlException
                            MessageBox.Show("Failed to insert data: " + ex.Message)
                        End Try
                    Catch ex As Exception
                        MsgBox("Error: " + ex.Message)
                    End Try
                End If



            End Using
        End Using
    End Sub

    Private Sub txtFullname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFullname.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsControl(e.KeyChar) Or Char.IsWhiteSpace(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtContact_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContact.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSalary_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSalary.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAge.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DataGridView2.Rows.Clear()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Emp_fullName`, `Emp_email`, `Emp_address`, `Emp_contact`, `Emp_type`, `Emp_salary`, `Emp_age`,`Emp_gender` FROM employee WHERE Emp_fullName LIKE @search OR Emp_email LIKE @search OR Emp_address LIKE @search OR Emp_contact LIKE @search OR Emp_type LIKE @search OR Emp_salary LIKE @search OR Emp_age LIKE @search OR Emp_gender LIKE @search", conn)
            cmd.Parameters.AddWithValue("@search", "%" & TextBox1.Text & "%")
            dr = cmd.ExecuteReader
            While dr.Read
                DataGridView2.Rows.Add(
                    DataGridView2.Rows.Count + 1,
                   dr("Emp_fullName"),
                    dr("Emp_email"),
                    dr("Emp_address"),
                    dr("Emp_contact"),
                    dr("Emp_type"),
                    dr("Emp_salary"),
                    dr("Emp_age"),
                    dr("Emp_gender"))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btn_find_Click(sender As Object, e As EventArgs) Handles btn_find.Click
        ClearData()
        btn_edit.Enabled = True


        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT * FROM `employee` WHERE `Emp_fullName`=@fullName", conn)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@fullName", txt_found.Text)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim fullName As String = dr.Item("Emp_fullName")
                Dim email As String = dr.Item("Emp_email")
                Dim address As String = dr.Item("Emp_address")
                Dim contact As String = dr.Item("Emp_contact")
                Dim type As String = dr.Item("Emp_type")
                Dim salary As Integer = dr.Item("Emp_salary")
                Dim age As Integer = dr.Item("Emp_age")
                Dim gender As String = dr.Item("Emp_gender")


                txtFullname.Text = fullName
                textEmail2.Text = email
                txtAddress.Text = address
                txtContact.Text = contact
                txtType.Text = type
                txtSalary.Text = salary
                txtAge.Text = age
                txtGender2.Text = gender


            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()

    End Sub

    Private Sub btn_edit_Click(sender As Object, e As EventArgs) Handles btn_edit.Click
   
        Try
            conn.Open()
            Dim cmd As New MySqlCommand("UPDATE `employee` SET `Emp_fullName`=@fullname,`Emp_email`=@email,`Emp_address`=@address,`Emp_contact`=@contact,`Emp_type`=@type,`Emp_salary`=@salary,`Emp_age`=@age,`Emp_gender`=@gender WHERE `Emp_fullName`=@fullname", conn)

            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@fullname", txtFullname.Text)
            cmd.Parameters.AddWithValue("@email", textEmail2.Text)
            cmd.Parameters.AddWithValue("@address", txtAddress.Text)
            cmd.Parameters.AddWithValue("@contact", txtContact.Text)
            cmd.Parameters.AddWithValue("@type", txtType.Text)
            cmd.Parameters.AddWithValue("@salary", txtSalary.Text)
            cmd.Parameters.AddWithValue("@age", txtAge.Text)
            cmd.Parameters.AddWithValue("@gender", txtGender2.Text)

            Dim i As Integer
            i = cmd.ExecuteNonQuery
            If i > 0 Then
                MsgBox("Food Edit Successfully !", vbInformation, "asd")
            Else
                MsgBox("Warning : Food Edit Failed !", vbCritical, "dasdsd")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
        ClearData()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        ClearData()
    End Sub
End Class


'CREATE TABLE employee (
'id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
' name VARCHAR(30) NOT NULL,
'email VARCHAR(50),
' phone VARCHAR(20),
' address VARCHAR(100),
'salary DECIMAL(10,2)
');'