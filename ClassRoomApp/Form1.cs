using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


namespace ClassRoomApp
{
    
    public partial class Form1 : Form
    {
        List<Student> studentsList = new List<Student>();

        string connectStr = "server = INSTRUCTORIT; database = ClassRoomApp; user ID = ProfileUser; password = ProfileUser2019";

        
    
        public Form1()
        {
            readAll();    
            InitializeComponent();
        }
        public void readAll()
        {
            
            using (SqlConnection myConnection = new SqlConnection(connectStr))
            {
                string sqlCommand;
                myConnection.Open();
                sqlCommand = "SELECT * FROM Students";
                SqlCommand myCommand = new SqlCommand(sqlCommand, myConnection);
                SqlDataReader myReader = myCommand.ExecuteReader(); ;

                while (myReader.Read())
                {
                    Student tempStudent = new Student();
                    tempStudent.ID = Convert.ToDouble(myReader["ID"]);
                    tempStudent.firstName = myReader["FirstName"].ToString();
                    tempStudent.lastName = myReader["LastName"].ToString();
                    tempStudent.email = myReader["email"].ToString();

                    studentsList.Add(tempStudent);

                }
                myReader.Close();
                myConnection.Close();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string sqlInsertCommand;
            using (SqlConnection myConn = new SqlConnection(connectStr))
            {
                Student addingStudent = new Student();
                addingStudent.ID = Convert.ToInt64(txtID.Text);
                addingStudent.firstName = txtFirstName.Text;
                addingStudent.lastName = txtLastName.Text;
                addingStudent.email = txtEmail.Text;

                studentsList.Add(addingStudent);

                myConn.Open();
                sqlInsertCommand = "INSERT INTO Students" + "(ID ,FirstName, LastName, Email) VALUES ("+ addingStudent.ID+ ",'" + addingStudent.firstName + "','" + addingStudent.lastName + "','" + addingStudent.email + "');";
                SqlCommand myInsertCommand = new SqlCommand(sqlInsertCommand, myConn);
                myInsertCommand.ExecuteNonQuery();
                myConn.Close();
            }
            
            MessageBox.Show("Student added successfully!");

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string sqlUpdateCommand;
            using (SqlConnection myConn = new SqlConnection(connectStr))
            {
                Student updateStudent = new Student();
                updateStudent.ID = Convert.ToInt64(txtID.Text);
                updateStudent.firstName = txtFirstName.Text;
                updateStudent.lastName = txtLastName.Text;
                updateStudent.email = txtEmail.Text;

                myConn.Open();
                sqlUpdateCommand = "UPDATE Students " + "SET FirstName = '" + updateStudent.firstName + "', LastName = '" + updateStudent.lastName + "', email = '" + updateStudent.email + "' WHERE ID =" + updateStudent.ID + ";";
                SqlCommand myInsertCommand = new SqlCommand(sqlUpdateCommand, myConn);
                myInsertCommand.ExecuteNonQuery();
                myConn.Close();
            }
            
            MessageBox.Show("Student updated successfully!");
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            string sqlDeleteCommand;
            using (SqlConnection myConn = new SqlConnection(connectStr))
            {
                Student deleteStudent = new Student();
                deleteStudent.ID = Convert.ToInt64(txtID.Text);
             

                myConn.Open();
                sqlDeleteCommand = "DELETE FROM Students WHERE ID =" + deleteStudent.ID + ";";
                SqlCommand myInsertCommand = new SqlCommand(sqlDeleteCommand, myConn);
                myInsertCommand.ExecuteNonQuery();
                myConn.Close();

      
            }
            
            MessageBox.Show("Student removed successfully!");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            int i;
            lstOutput.Items.Clear();
            for (i = 0; i < studentsList.Count; i++)
            {
                Student tempStudent = new Student();

                tempStudent = studentsList[i];
                
                lstOutput.Items.Add("ID = " + tempStudent.ID.ToString()+ " FirstName = " + tempStudent.firstName + " LastName = " + tempStudent.lastName + " Email = " + tempStudent.email);  
            }
                
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string sqlFilterCommand;
            List<Student> selectedStudents = new List<Student>();
            int j;
            using (SqlConnection myConn = new SqlConnection(connectStr))
            {
                Student filterStudent = new Student();
                filterStudent.lastName = txtLastFilter.Text;


                myConn.Open();
                sqlFilterCommand = "SELECT * FROM Students WHERE LastName ='" + filterStudent.lastName + "';";
                SqlCommand myFilterCommand = new SqlCommand(sqlFilterCommand, myConn);
                SqlDataReader myFilterReader = myFilterCommand.ExecuteReader(); ;

                while (myFilterReader.Read())
                {
                    Student tempStudent = new Student();
                    tempStudent.ID = Convert.ToDouble(myFilterReader["ID"]);
                    tempStudent.firstName = myFilterReader["FirstName"].ToString();
                    tempStudent.lastName = myFilterReader["LastName"].ToString();
                    tempStudent.email = myFilterReader["email"].ToString();

                    selectedStudents.Add(tempStudent);

                }
                myFilterReader.Close();
                myConn.Close();

                
                lstOutput.Items.Clear();
                for (j = 0; j < selectedStudents.Count; j++)
                {
                    Student tempStudent = new Student();

                    tempStudent = selectedStudents[j];

                    lstOutput.Items.Add("ID = " + tempStudent.ID.ToString() + " FirstName = " + tempStudent.firstName + " LastName = " + tempStudent.lastName + " Email = " + tempStudent.email);
                }

            }
            
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            string pathName = @"C:\Andres";

            string fileName = @"Students.txt";
            int k;
            StreamWriter sw = new StreamWriter(Path.Combine(pathName, fileName),false);

            for (k = 0; k < studentsList.Count; k++)
            {
                Student tempStudent = new Student();

                tempStudent = studentsList[k];
                sw.WriteLine("ID = " + tempStudent.ID.ToString() + " FirstName = " + tempStudent.firstName + " LastName = " + tempStudent.lastName + " Email = " + tempStudent.email);
                
            }
            sw.Close();
            MessageBox.Show("Students saved!");

        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            string pathName = @"C:\Andres";

            string fileName = @"Students.txt";

            string pfn = Path.Combine(pathName, fileName);

            if (File.Exists(pfn)==true)
            {
                StreamReader sr = new StreamReader(pfn);
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    lstOutput.Items.Add(line);
                    
                }
                sr.Close();
            }
            
        }
    }
}
