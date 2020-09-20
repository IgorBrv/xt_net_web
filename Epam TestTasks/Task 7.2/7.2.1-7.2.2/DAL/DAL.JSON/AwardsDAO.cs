using System;
using System.Collections.Generic;
using InterfaceDAL;
using Entities;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace JsonDAL
{   // Объекты DAL слоя

	public class AwardsDAO : IAwardsDAO
	{   // Объект DAO отвечающий за работу с сущностью наград

		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public string Path { get; set; }

		public List<Award> GetAllAwards()
		{
			try
			{
				List<Award> awardsList = new List<Award>();

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllAwards";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						awardsList.Add(new Award(Guid.Parse(reader["id"] as string), reader["name"] as string, reader["emblempath"] as string));
					}
				}

				return awardsList;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public bool IsAwardInStorage(Guid id)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "AwardsNumAtStorage";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id.ToString());

					command.Parameters.Add(idParam);

					connection.Open();

					if ((int)command.ExecuteScalar() > 0)
					{
						return true;
					}

					return false;
				}
			}
			catch (SqlException)
			{
				return false;
			}
		}

		public bool AddAward(Award award)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "AddAward";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", award.id.ToString());
					SqlParameter nameParam = new SqlParameter("@name", award.title);
					SqlParameter emblempathParam = new SqlParameter
					{
						ParameterName = "@emblempath",
						Value = award.emblempath,
						SqlDbType = SqlDbType.NVarChar,
						IsNullable = true
					};

					command.Parameters.Add(idParam);
					command.Parameters.Add(nameParam);
					command.Parameters.Add(emblempathParam);

					connection.Open();
					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}


		public bool RemoveAward(Award award) => RemoveAwardByID(award.id);


		public bool RemoveAwardByID(Guid id)
		{
			try
			{
				Award award = GetAwardByID(id);

				if (award.emblempath != null)
				{
					File.Delete($"{Path}{award.emblempath.Substring(1)}");
				}

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemoveAward";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id);

					command.Parameters.Add(idParam);

					connection.Open();

					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}


		public bool UpdateAward(Award award)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "UpdateAward";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", award.id);
					SqlParameter nameParam = new SqlParameter("@name", award.title);
					SqlParameter emblempathParam = new SqlParameter
					{
						ParameterName = "@emblempath",
						Value = award.emblempath,
						SqlDbType = SqlDbType.NVarChar,
						IsNullable = true
					};

					command.Parameters.Add(idParam);
					command.Parameters.Add(nameParam);
					command.Parameters.Add(emblempathParam);

					connection.Open();

					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}

		public Award GetAwardByID(Guid id)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAward";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id.ToString());

					command.Parameters.Add(idParam);

					connection.Open();

					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						return new Award(Guid.Parse(reader["id"] as string), reader["name"] as string, reader["emblempath"] as string);
					}
				}

				return null;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public string AddEmblemToAward(Guid id, string ext, BinaryReader br)
		{
			string emblemName = $"{Guid.NewGuid()}.{ext}";
			string savePath = $"{Path}\\images\\avatars\\";

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

			try
			{
				using (br)
				{
					int lenght = (int)br.BaseStream.Length;
					byte[] file = br.ReadBytes(lenght);
					File.WriteAllBytes($"{savePath}{emblemName}", file);
				}

				Award award = GetAwardByID(id);

				if (award.emblempath != null)
				{
					File.Delete($"{Path}{award.emblempath.Substring(1)}");
				}

				award.emblempath = $"./images/avatars/{emblemName}";

				UpdateAward(award);

				return award.emblempath;

			}
			catch (IOException)
			{
				return null;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public bool RemoveEmblemFromAward(Guid id)
		{
			try
			{
				Award award = GetAwardByID(id);

				if (award.emblempath != null)
				{
					File.Delete($"{Path}{award.emblempath.Substring(1)}");
				}

				award.emblempath = null;

				UpdateAward(award);

				return true;
			}
			catch (IOException)
			{
				return false;
			}
		}
	}
}