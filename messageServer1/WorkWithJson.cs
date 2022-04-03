using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace messageServer1
{
    /// <summary>
    /// Класс для работы с JSON файлами.
    /// </summary>
    public static class WorkWithJson
    {
        /// <summary>
        /// Запиcь в JSON файл списка обьектов.
        /// </summary>
        /// <typeparam name="T">User или Message.</typeparam>
        /// <param name="objects">Лист объектов.</param>
        /// <param name="path">Путь до файла.</param>
        public static void WriteToJson<T>(List<T> objects, string path)
        {
            DataContractJsonSerializer serializer = new(typeof(List<T>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.WriteObject(fs, objects);
            }
        }

        /// <summary>
        /// Чтение из JSON файла списка объектов.
        /// </summary>
        /// <typeparam name="T">User или Message.</typeparam>
        /// <param name="path">Путь до файла.</param>
        /// <returns>Лист объектов.</returns>
        public static (bool,string,List<T>) ReadFromJson<T>(string path)
        {
            List<T> objects = new List<T>();
            try
            {
                DataContractJsonSerializer deserializer = new(typeof(List<T>));
                objects = new List<T>();
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    objects = deserializer.ReadObject(fs) as List<T>;
                }
            }
            catch
            {
                return (false,"Ошибка при чтении из файла.Файл пуст или удален.\n" +
                    "Сначала проинициализируйте файлы.", objects);
            }
            return (true, "", objects);
        }
    }
}
