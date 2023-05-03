Imports MySql.Data.MySqlClient
Public Class Employee


    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Panel1.Left = (Me.Width - Panel1.Width) / 2
    End Sub



    Private Sub save_Click(sender As Object, e As EventArgs) Handles saveBtn.Click
        Dim connString As String = "server=localhost;user=root;password=;database=dad's buffalo restobar"
        Dim conn As New MySqlConnection(connString)
        Dim cmd As New MySqlCommand()
        cmd.Connection = conn
        cmd.CommandText = "INSERT INTO employee(Emp_fullName, Emp_email, Emp_address, Emp_contact,Emp_type,Emp_salary,Emp_age,Emp_gender) VALUES(@fullname, @email, @address,@contact,@type, @salary,@age,@gender)"
        cmd.Parameters.AddWithValue("@fullname", nameTxt.Text)
        cmd.Parameters.AddWithValue("@email", txtEmail.Text)
        cmd.Parameters.AddWithValue("@address", txtAdd.Text)
        cmd.Parameters.AddWithValue("@contact", contxt.Text)
        cmd.Parameters.AddWithValue("@type", typeTxt.Text)
        cmd.Parameters.AddWithValue("@salary", salaryTxt.Text)
        cmd.Parameters.AddWithValue("@age", AgeTxt.Text)
        cmd.Parameters.AddWithValue("@gender", txtGender.Text)

        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            MessageBox.Show("Employee inserted successfully")
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            conn.Close()
        End Try

    End Sub


    Public Function GetNextUserId() As Integer
        con.ConnectionString = "server=localhost;username=root;password=;database=dad's buffalo restobar"
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT MAX(id) FROM employee"
        cmd.Connection = con

        Dim nextId As Integer = 0
        Try
            con.Open()
            Dim result = cmd.ExecuteScalar()
            If result IsNot DBNull.Value AndAlso result IsNot Nothing Then
                nextId = Convert.ToInt32(result) + 1
            Else
                nextId = 1
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Return nextId
    End Function


End Class


'CREATE TABLE employee (
'id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
' name VARCHAR(30) NOT NULL,
'email VARCHAR(50),
' phone VARCHAR(20),
' address VARCHAR(100),
'salary DECIMAL(10,2)
');'