using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ReadToday.Model;

namespace ReadToday.DataAccess
{
    public class CFileDataService : IDataService
    {
        private const String StorageFile = "Books.json";

        public IEnumerable<CLookupItem> GetAllBooks()
        {
            return ReadFromFile()
                .Select(b => CLookupItem.Create(b.Id, b.Title));
        }

        public CBook GetBookById(Guid bookId)
        {
            List<CBook> books = ReadFromFile();
            return books.Single(f => f.Id == bookId);
        }

        public void SaveBook(CBook book)
        {
            List<CBook> books = ReadFromFile();
            CBook existing = books.SingleOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                books.Remove(existing);
            }

            books.Add(book);
            SaveToFile(books);
        }

        public void DeleteBook(Guid bookId)
        {
            List<CBook> books = ReadFromFile();
            CBook existing = books.Single(b => b.Id == bookId);
            books.Remove(existing);
            SaveToFile(books);
        }

        private void UpdateBook(CBook book)
        {
            List<CBook> books = ReadFromFile();
            CBook existing = books.Single(b => b.Id == book.Id);
            Int32 indexOfExisting = books.IndexOf(existing);
            books.Insert(indexOfExisting, book);
            books.Remove(existing);
            SaveToFile(books);
        }

        private void InsertBook(CBook book)
        {
            List<CBook> books = ReadFromFile();
        //    book.Id = Guid.NewGuid();
            books.Add(book);
            SaveToFile(books);
        }

        private void SaveToFile(List<CBook> bookList)
        {
            String json = JsonConvert.SerializeObject(bookList, Formatting.Indented);
            File.WriteAllText(StorageFile, json);
        }

        private List<CBook> ReadFromFile()
        {
            if (!File.Exists(StorageFile))
            {
                return new List<CBook>();
            }

            String json = File.ReadAllText(StorageFile);
            return JsonConvert.DeserializeObject<List<CBook>>(json);
        }

        public void Dispose()
        {
        }
    }
}
