# ADO.NET-Async-Await

## Simple Async/Await method

```C#
public async Task FillComboBoxAsync()
{
     SqlConnection con1;
     using (con1 = new SqlConnection(cs))
     {
          await con1.OpenAsync();
          string sql = "select FirstName + ' ' + LastName from book.Books join book.Authors on book.Books.Id_Author =                               book.Authors.Id_Author;";

               SqlCommand com = new SqlCommand(sql, con1);
               SqlDataReader reader = await com.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                    comboBox1.Items.Add(reader[0]);
       }
}```
