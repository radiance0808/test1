using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using test1.Model;

namespace test1.Services
{
    public class TeamMemberService : GetTeamMember
    {

        public TeamMember getTeamMember(int id)
        {
            var teamMember = new TeamMember();
            var tasksList = new List<Model.Task>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19230;Integrated Security=True"))
            {

                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "SELECT FIRSTNAME,LASTNAME,EMAIL FROM TEAMMEMBER WHERE IDTEAMMEMBER = @IDMEMBER";
                    com.Parameters.AddWithValue("IDMEMBER", id);
                    con.Open();

                    var dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        teamMember.FirstName = dr["firstname"].ToString();
                        teamMember.LastName = dr["lastname"].ToString();
                        teamMember.Email = dr["email"].ToString();
                        dr.Close();
                    }
                    else
                    {
                        throw new ArgumentException("no such id");
                    }

                    com.CommandText = "SELECT NAME,DESCRIPTION,DEADLINE,IDPROJECT,IDTASKTYPE FROM TEAMMEMBER, TASK WHERE TASK.IDASSIGNEDTO=TEAMMEMBER.IDTEAMMBER AND TASK.IDCREATOR=TEAMMEMBER.IDTEAMMBER AND IDTEAMMEMBER=@ID ORDER BY DEADLINE DESC";
                    com.Parameters.AddWithValue("id", id);
                    dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        var task = new Model.Task();
                        task.Name = dr["Name"].ToString();
                        task.Description = dr["Description"].ToString();
                        task.DeadLine = dr["Deadline"].ToString();
                        task.ProjectId = (int)dr["IdProject"];
                        task.TypeId = (int)dr["IdTaskType"];
                        tasksList.Add(task);
                    }
                    teamMember.Tasks = tasksList;
                }
            }
            return teamMember;
        }


        public void deleteProject(ProjNameReq project)
        {
            using (SqlConnection con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19230;Integrated Security=True"))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                SqlTransaction transaction = con.BeginTransaction();
                com.Transaction = transaction;

                com.CommandText = "SELECT IDPROJECT FROM PROJECT WHERE NAME=@NAME";
                com.Parameters.AddWithValue("projectname", project.ProjectName);
                var dr = com.ExecuteReader();
                var projModel = new Model.Project();
                if (dr.Read())
                {
                    projModel.IdProject = (int)dr["IdProject"];
                    dr.Close();
                }
                else
                {
                    throw new ArgumentException("no such project");
                }

                com.CommandText = "SELECT * FROM PROJECT,TASK WHERE PROJECT.IDPROJECT=TASK.IDPROJECT AND PROJECT.NAME=@NAME";
                com.Parameters.AddWithValue("projectname", project.ProjectName);

                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    com.CommandText = "DELETE FROM TASK WHERE IDPROJECT = @IDMEMBER";
                    com.Parameters.AddWithValue("idMember", projModel.IdProject);
                    com.CommandText = "DELETE FROM PROJECT WHERE NAME = @NAME";
                    com.Parameters.AddWithValue("name", project.ProjectName);
                }

                else
                {
                    throw new ArgumentException("no such project");
                }

                com.ExecuteNonQuery();
                transaction.Commit();
            }
        }


    }
}

