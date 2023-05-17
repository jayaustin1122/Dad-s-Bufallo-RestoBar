
Imports System.IO
Imports System.Drawing
Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class Form2
    Private WithEvents pan As Panel
    Private WithEvents pan_top As Panel
    Private WithEvents foodcode As Label
    Private WithEvents foodname As Label
    Private WithEvents price As Label
    Private WithEvents img As PictureBox
    Private currentTime As DateTime ' declare a variable to store the current time
    ' Create a CultureInfo object that represents the Philippines
    Dim phCulture As New CultureInfo("fil-PH")

    ' Set the format and format provider of the price column to display the peso sign

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        currentTime = DateTime.Now ' update the value of the currentTime variable with the current time
        Time.Text = currentTime.ToString("hh:mm:ss tt") ' update the text of the Label control with the value of the currentTime variable
        Get_grandTotal()
        Get_pricedata()
    End Sub
    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Panel1.Left = (Me.Width - Panel1.Width) / 2
    End Sub
   
    Private Sub Drinks_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconn()
        txt_transno.Enabled = False
        DataGridView1.RowTemplate.Height = 30
        DataGridView1.Columns("Column4").DefaultCellStyle.Format = "C"
        DataGridView1.Columns("Column6").DefaultCellStyle.Format = "C"
        Timer1.Interval = 1000 ' set the interval of the Timer control to 1000 milliseconds (1 second)
        Timer1.Start() ' start the Timer control
        Label4.Text = Date.Now.ToString("ddd, dd-MM-yyyy")
        Load_Foods()
        auto_Transno()
        DisplayNameEmployee()
        textBoxAmount.Enabled = False

    End Sub
    Sub auto_Transno()
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT * FROM `ordered` order by Or_date desc", conn)
            dr = cmd.ExecuteReader
            dr.Read()
            If dr.HasRows = True Then
                txt_transno.Text = dr.Item("Or_transno").ToString + 1

            Else
                txt_transno.Text = Date.Now.ToString("yyyyMM") & "001"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub


    Sub Load_Foods()
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.AutoScroll = True
        Try
            conn.Open()
            cmd = New MySqlCommand("SELECT `Fo_img`, `Fo_foodcode`, `Fo_name`, `Fo_price`, `Fo_qty` FROM `foods`", conn)
            dr = cmd.ExecuteReader
            While dr.Read
                Load_controls()
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        conn.Close()
    End Sub




    Sub Load_controls()
        Dim len As Long = dr.GetBytes(0, 0, Nothing, 0, 0)
        Dim array(CInt(len)) As Byte
        dr.GetBytes(0, 0, array, 0, CInt(len))

        pan = New Panel
        With pan
            .Width = 170
            .Height = 190
            .BackColor = Color.FromArgb(0, 192, 192)
            .Tag = dr.Item("Fo_foodcode").ToString
        End With
        pan_top = New Panel
        With pan_top
            .Width = 170
            .Height = 10
            .BackColor = Color.FromArgb(0, 192, 192)
            .Dock = DockStyle.Top
            .Tag = dr.Item("Fo_foodcode").ToString
        End With

        img = New PictureBox
        With img
            .Height = 120
            .BackgroundImageLayout = ImageLayout.Stretch
            .Dock = DockStyle.Top
            .Tag = dr.Item("Fo_foodcode").ToString
        End With


        foodname = New Label
        With foodname
            .ForeColor = Color.White
            .Font = New Font("Segoe UI", 8, FontStyle.Bold)
            .TextAlign = ContentAlignment.MiddleLeft
            .Dock = DockStyle.Top
            .Tag = dr.Item("Fo_foodcode").ToString
        End With

        price = New Label
        With price
            .ForeColor = Color.White
            .Font = New Font("Segoe UI", 8, FontStyle.Bold)
            .TextAlign = ContentAlignment.MiddleLeft
            .Dock = DockStyle.Top
            .Tag = dr.Item("Fo_foodcode").ToString
        End With
        Dim ms As New System.IO.MemoryStream(array)
        Dim bitmap As New System.Drawing.Bitmap(ms)
        img.BackgroundImage = bitmap


        foodname.Text = " Drink Name : " & dr.Item("Fo_name").ToString
        price.Text = " Price             : ₱ " & dr.Item("Fo_price").ToString

        pan.Controls.Add(price)
        pan.Controls.Add(foodname)
        pan.Controls.Add(foodcode)
        pan.Controls.Add(img)


        pan.Controls.Add(pan_top)
        FlowLayoutPanel1.Controls.Add(pan)


        AddHandler foodname.Click, AddressOf Selectimg_Click
        AddHandler price.Click, AddressOf Selectimg_Click
        AddHandler img.Click, AddressOf Selectimg_Click
        AddHandler pan.Click, AddressOf Selectimg_Click


    End Sub

    Public Sub Selectimg_Click(sender As Object, e As EventArgs)
        conn.Open()
        Try
            cmd = New MySqlCommand("SELECT `Fo_foodcode`, `Fo_name`, `Fo_price` FROM `foods` WHERE `Fo_foodcode` like '" & sender.tag.ToString & "%' ", conn)
            dr = cmd.ExecuteReader
            While dr.Read
                Dim exist As Boolean = False, numrow As Integer = 0, numtext As Integer
                For Each itm As DataGridViewRow In DataGridView1.Rows
                    If itm.Cells(1).Value IsNot Nothing Then
                        If itm.Cells(1).Value.ToString = dr.Item("Fo_foodcode") Then
                            exist = True
                            numrow = itm.Index
                            numtext = CInt(itm.Cells(4).Value)
                            Exit For
                        End If
                    End If
                Next
                If exist = False Then
                    Dim price As Decimal = dr("Fo_price")
                    Dim subtotalprice As Double
                    subtotalprice = price * 1
                    DataGridView1.Rows.Add(DataGridView1.Rows.Count + 1,
                                           dr.Item("Fo_foodcode"),
                                           dr.Item("Fo_name"),
                                           dr.Item("Fo_price"),
                                           1, subtotalprice)

                Else
                    DataGridView1.Rows(numrow).Cells(4).Value = CInt("1") + numtext
                    DataGridView1.Rows(numrow).Cells(5).Value = DataGridView1.Rows(numrow).Cells(3).Value * DataGridView1.Rows(numrow).Cells(4).Value

                End If

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        dr.Dispose()
        conn.Close()

    End Sub

    Sub Get_grandTotal()
        Try
            Dim grandtotal As Double = 0
            For i As Double = 0 To DataGridView1.Rows.Count() - 1 Step +1
                grandtotal = grandtotal + DataGridView1.Rows(i).Cells(5).Value

            Next
            lbl_GrandTotal.Text = Format(CDec(grandtotal), " #,##0.00")

        Catch ex As Exception

        End Try
    End Sub

    Sub Get_pricedata()
        Try
            Dim noofProducts As Double = 0

            For Each row As DataGridViewRow In DataGridView1.Rows ' iterate through each row in the grid
                Dim price As Decimal = CDec(row.Cells("Column5").Value) ' retrieve the value of the PriceColumn cell and convert it to a Decimal
                noofProducts += price ' add the price to the total variable
            Next

            lbl_noOfProducts.Text = noofProducts
        Catch ex As Exception

        End Try

    End Sub

   


  
    Sub new_order()
        Load_Foods()
        DataGridView1.Rows.Clear()
        auto_Transno()
        textBoxAmount.Clear()
        textBoxRecieve.Clear()
        Customertextbox.Clear()
        txtAdd.Clear()
        contxt.Clear()
        AgeTxt.Clear()
        txtEmail.Clear()



    End Sub

    Sub saveCustomer()

        Dim connString As String = "server=localhost;user=root;password=;database=dad's buffalo restobar"

        Using conn As New MySqlConnection(connString)
            Using cmd As New MySqlCommand()
                cmd.Connection = conn

                Try
                    cmd.CommandText = "INSERT INTO costumer(Co_fullName, Co_email, Co_contact, Co_age,Co_address) VALUES(@fullname, @email, @phone,@age,@address)"
                    cmd.Parameters.AddWithValue("@fullname", Customertextbox.Text)
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text)
                    cmd.Parameters.AddWithValue("@address", txtAdd.Text)
                    cmd.Parameters.AddWithValue("@phone", contxt.Text)
                    cmd.Parameters.AddWithValue("@age", AgeTxt.Text)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Ordered successfully!")
                    Catch ex As MySqlException
                        MessageBox.Show("Failed to insert data: " + ex.Message)
                    End Try
                Catch ex As Exception
                    MsgBox("Error: " + ex.Message)
                End Try

            End Using
        End Using

    End Sub





    Sub DisplayNameEmployee()
        Dim connection As New MySqlConnection("server=localhost;user=root;password=;port=3306;database=dad's buffalo restobar")
        Dim commandText As String = "SELECT * FROM employee"
        Dim command As New MySqlCommand(commandText, connection)

        Dim adapter As New MySqlDataAdapter(command)
        Dim dataTable As New DataTable()
        adapter.Fill(dataTable)

        Emp.DataSource = dataTable
        Emp.DisplayMember = "Emp_fullName"
        Emp.ValueMember = "Emp_id"

    End Sub





    Private Sub UpdateInventoryFromOrder(dataGridView As DataGridView)


        ' Loop through each row in the DataGridView and update the inventory for each product
        For Each row As DataGridViewRow In DataGridView1.Rows
            Dim productId As Integer = CInt(row.Cells("Column2").Value)
            Dim quantityOrdered As Integer = CInt(row.Cells("Column5").Value)

            ' Retrieve the current inventory quantity for the product
            Dim currentQuantity As Integer = GetInventoryQuantity(productId)


            ' Calculate the new inventory quantity after subtracting the quantity ordered
            Dim newQuantity As Integer = currentQuantity - quantityOrdered
            ' Update the inventory quantity in the database
            UpdateInventoryQuantity(productId, newQuantity)
        Next
    End Sub

    ' Get the current inventory quantity for the specified product ID
    Private Function GetInventoryQuantity(productId As Integer) As Integer
        Dim connection As New MySqlConnection("server=localhost;user=root;password=;port=3306;database=dad's buffalo restobar")
        Dim commandText As String = "SELECT Fo_qty FROM foods WHERE Fo_foodcode = @productId"
        Dim command As New MySqlCommand(commandText, connection)
        command.Parameters.AddWithValue("@productId", productId)

        Try
            connection.Open()
            Dim result As Object = command.ExecuteScalar()
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                Return CInt(result)
            Else
                Return 0
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return 0
        Finally
            connection.Close()
        End Try
    End Function

    ' Update the inventory quantity for the specified product ID
  Private Sub UpdateInventoryQuantity(productId As Integer, newQuantity As Integer)
        Try
            Dim cmd As New MySqlCommand("UPDATE `foods` SET `Fo_qty` = @quantity WHERE `Fo_foodcode`= @foodcode", conn)
            cmd.Parameters.Clear()
            cmd.Connection = conn
            cmd.Parameters.AddWithValue("@quantity", newQuantity)
            cmd.Parameters.AddWithValue("@foodcode", productId)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub



    'digit
    Private Sub contxt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles contxt.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    'number
    Private Sub Customertextbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Customertextbox.KeyPress
        If Not (Char.IsLetter(e.KeyChar) Or Char.IsControl(e.KeyChar) Or Char.IsWhiteSpace(e.KeyChar)) Then
            e.Handled = True
        End If

    End Sub

  
    Private Sub AgeTxt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles AgeTxt.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub textBoxRecieve_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)
        new_order()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs)
        new_order()
    End Sub

    Private Sub Button10_Click_1(sender As Object, e As EventArgs) Handles Button10.Click
        Manage_Foods_Real.ShowDialog()
    End Sub


    Private Sub textBoxRecieve_TextChanged_1(sender As Object, e As EventArgs) Handles textBoxRecieve.TextChanged
        Dim equal As Double = 0
        Try
            Dim grandtotal As Double = 0
            For i As Double = 0 To DataGridView1.Rows.Count() - 1 Step +1
                grandtotal = grandtotal + DataGridView1.Rows(i).Cells(5).Value

            Next
            If String.IsNullOrEmpty(textBoxRecieve.Text) OrElse Not Decimal.TryParse(textBoxRecieve.Text, Nothing) Then
                textBoxAmount.Text = ""
            Else
                equal = textBoxRecieve.Text - Format(CDec(grandtotal), "#,##0.00")
                textBoxAmount.Text = equal
            End If



        Catch ex As Exception

        End Try
    End Sub
    Function getCustomerId() As Integer
        Dim finalId As Integer = 0
        conn.Open()
        Try
            cmd.CommandText = "SELECT MAX(Co_id) FROM costumer"
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

    Private Sub Button15_Click_1(sender As Object, e As EventArgs) Handles Button15.Click
        Dim nextId As Integer = getCustomerId()
        If MsgBox("Are You Sure?", vbQuestion + vbYesNo) = vbYes Then
            If textBoxRecieve.Text = String.Empty Then
                MsgBox("Please Enter Amount !", vbExclamation)
                Return
            ElseIf textBoxRecieve.Text < 0 Then
                MsgBox("Not Enough Balance !" & vbNewLine & textBoxRecieve.Text & " ₱", MsgBoxStyle.Exclamation)
                Return
            ElseIf Customertextbox.Text = "" Or txtEmail.Text = "" Or txtAdd.Text = "" Or contxt.Text = "" Or AgeTxt.Text = "" Then
                MsgBox("Information Missing")
                Return
            Else
                Try

                    conn.Open()
                    cmd = New MySqlCommand("INSERT INTO `ordered`(`Co_id`, `Fo_id`, `Or_qty`, `Or_cost`, `Or_transNo`, `Or_date`) VALUES (@Coid, @Foid, @Orqty, @Orcost, @OrtransNo, @Ordate)", conn)
                    For j As Integer = 0 To DataGridView1.Rows.Count - 1 Step +1
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@Coid", nextId)
                        cmd.Parameters.AddWithValue("@Foid", DataGridView1.Rows(j).Cells(1).Value)
                        cmd.Parameters.AddWithValue("@Orqty", lbl_noOfProducts.Text)
                        cmd.Parameters.AddWithValue("@Orcost", lbl_GrandTotal.Text)
                        cmd.Parameters.AddWithValue("@OrtransNo", txt_transno.Text)
                        cmd.Parameters.AddWithValue("@Ordate", CDate(Label4.Text))

                        i = cmd.ExecuteNonQuery()
                    Next
                    cmd = New MySqlCommand("INSERT INTO `serve`(`Emp_id`, `Co_id`, `Se_date`) VALUES (@empId,@coId,@sedate)", conn)
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@empId", Emp.SelectedValue)
                    cmd.Parameters.AddWithValue("@coId", nextId)
                    cmd.Parameters.AddWithValue("@sedate", CDate(Label4.Text))
                    cmd.ExecuteNonQuery()
                    If i > 0 Then

                        UpdateInventoryFromOrder(DataGridView1)
                        saveCustomer()

                    End If
                Catch ex As Exception
                    MsgBox("Error: " & ex.Message)
                    Return
                Finally
                    conn.Close()
                End Try
            End If
        Else
            Return
        End If
        new_order()

        
    End Sub


    Private Sub Button14_Click_1(sender As Object, e As EventArgs) Handles Button14.Click
        If MsgBox("Are you sure Exit !", vbQuestion + vbYesNo) = vbYes Then
            End
        Else
            Return
        End If
        End
    End Sub



End Class