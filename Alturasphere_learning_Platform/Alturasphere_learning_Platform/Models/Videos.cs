using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Alturasphere_learning_Platform.Models
{
    public class Videos
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Video name is required.")]
        [StringLength(100, ErrorMessage = "Video name cannot exceed 100 characters.")]
        [Display(Name = "Video Name")]
        public string VideoName { get; set; }

        [Required(ErrorMessage = "Please upload a video.")]
        //[FileExtensions(Extensions = "mp4,avi,mkv,flv", ErrorMessage = "Only video files with the extensions .mp4, .avi, .mkv, .flv are allowed.")]
        //[FileExtensions(Extensions = "mp4,avi,mkv,flv", ErrorMessage = "Only video files with the extensions .mp4, .avi, .mkv, .flv are allowed.")]
        [DataType(DataType.Upload)] // This helps with client-side validation
        [Display(Name = "Video")]
        public HttpPostedFileBase video { get; set; }

    }
    public class VideoOperation
    {
        SqlConnection con = new SqlConnection("Data Source=SUSHANT\\MSSQLSERVER01;Initial Catalog=Altura_Learning_Website;Integrated Security=True;Encrypt=False");
        SqlCommand cmd;
        string Query;

        // Method to upload the video (converted to byte[] format)
        public int UploadVideo(Videos E)
        {
            // Convert the uploaded video file into a byte array
            byte[] videoData;
            using (var reader = new BinaryReader(E.video.InputStream))
            {
                videoData = reader.ReadBytes(E.video.ContentLength);
            }

            // Define the query to insert data into the Courses table
            Query = "INSERT INTO Courses (course_name, video_name, video) VALUES (@CourseName, @VideoName, @Video)";

            // Create SqlCommand object
            cmd = new SqlCommand(Query, con);
            cmd.CommandType = CommandType.Text;

            // Add parameters to the SQL command
            cmd.Parameters.AddWithValue("@CourseName", E.CourseName);
            cmd.Parameters.AddWithValue("@VideoName", E.VideoName);
            cmd.Parameters.AddWithValue("@Video", videoData); // Pass video data as byte array

            try
            {
                // Open the connection
                con.Open();

                // Execute the query and return the number of rows affected
                int result = cmd.ExecuteNonQuery();

                return result; // Return the result (number of rows affected)
            }
            catch (Exception ex)
            {
                // Handle errors (you can log the error or throw a custom exception)
                Console.WriteLine("Error uploading video: " + ex.Message);
                return -1; // Return a failure indicator
            }
            finally
            {
                // Ensure that the connection is closed
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        // Method to retrieve the video binary data from the database
        public byte[] GetVideo(int courseId)
        {
            Query = "SELECT video FROM Courses WHERE course_id = @CourseId";
            byte[] videoData = null;

            cmd = new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@CourseId", courseId);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        videoData = reader["video"] as byte[]; // Retrieve the video as byte array
                    }
                }

                reader.Close();
                return videoData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving video: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
