using CsvHelper;
using SqliteProvider.Models;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace SqliteProvider.Repositories
{
	public class CsvService
	{
		string inputFile = @"C:\CsvReader";
		private const string dbPath = "database.db";

		public List<Organization> ReadCsv()
		{
			List<Organization> outputRecords = new List<Organization>();
			string[] csvFileArr = Directory.GetFiles(inputFile);
			foreach (var item in csvFileArr)
			{
				using var reader = new StreamReader(item);
				using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
				var records = csv.GetRecords<Organization>();

				foreach (var record in records)
				{
					outputRecords.Add(record);
				}
			}

			return outputRecords;
		}

		public void SaveToDatabase(List<Organization> records)
		{
			using (SQLiteConnection connection = new SQLiteConnection($"Data Source = {dbPath}"))
			{
				connection.Open();

				using (var cmd = new SQLiteCommand(connection))
				{
					
					using (var transaction = connection.BeginTransaction())
					{
						try
						{
							cmd.CommandText = "INSERT INTO Organizations(\"Index\", Organization_Id, Name, Website, Country, Description, Founded, Industry, Number_Of_Employees) " +
											  "VALUES(@Index, @Organization_Id, @Name, @Website, @Country, @Description, @Founded, @Industry, @Number_Of_Employees)";

							cmd.Parameters.Add("@Index", DbType.String);
							cmd.Parameters.Add("@Organization_Id", DbType.String);
							cmd.Parameters.Add("@Name", DbType.String);
							cmd.Parameters.Add("@Website", DbType.String);
							cmd.Parameters.Add("@Country", DbType.String);
							cmd.Parameters.Add("@Description", DbType.String);
							cmd.Parameters.Add("@Founded", DbType.String);
							cmd.Parameters.Add("@Industry", DbType.String);
							cmd.Parameters.Add("@Number_Of_Employees", DbType.Int32);

							foreach (var record in records)
							{
								cmd.Parameters["@Index"].Value = record.Index;
								cmd.Parameters["@Organization_Id"].Value = record.Organization_Id;
								cmd.Parameters["@Name"].Value = record.Name;
								cmd.Parameters["@Website"].Value = record.Website;
								cmd.Parameters["@Country"].Value = record.Country;
								cmd.Parameters["@Description"].Value = record.Description;
								cmd.Parameters["@Founded"].Value = record.Founded;
								cmd.Parameters["@Industry"].Value = record.Industry;
								cmd.Parameters["@Number_Of_Employees"].Value = record.Number_Of_Emplooyes;

								cmd.ExecuteNonQuery();
							}

							transaction.Commit();
						}
						catch (Exception)
						{
							transaction.Rollback();
							throw;
						}
					}
				}
			}
		}


		public void ProcessCsv()
		{
			var records = ReadCsv();
			SaveToDatabase(records);
		}
	}
}
