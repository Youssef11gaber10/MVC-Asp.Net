using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSetttings//no need to create obj from it
    {
        //upload
        public static string UploadFile(IFormFile file, string FolderName)//this "file" may be image , text, docs,pdf ,whatever ,so must send folder name to catogorize file if image will save in subfolder images
        {
            //1.Get Located Folder Path => the path the file will store 
            //                  E:\C#\MVC\Session-3-MVC\Demo.PL\+  wwwroot\Files\      +   Images\
            //                  Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + FolderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);


            //2.Get File Name and Make it Unique
            // string FileName = file.Name;//return its type
            //string FileName = $"{Guid.NewGuid()}{file.FileName}";//return its name
            string FileName = $"{file.FileName}";//return its name


            //3.Get File Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            //4.Save File As Streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            //5.Return File Name
            return FileName;



        }


        //delete
        public static void DeleteFile(string FileName, string FolderName)
        {
            //1.Get File Path             //until PL then wwwroot\\files...
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);
            //2.Check if File Exists Or Not
            if (File.Exists(FilePath))
            {
                // If Exists Remove It
                File.Delete(FilePath);
            }

        }






    }
}
