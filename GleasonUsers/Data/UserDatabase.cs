using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GleasonUsers.Model;
using SQLite;

namespace GleasonUsers.Data
{
    public class UserDatabase
    {
        readonly SQLiteAsyncConnection database;

        public UserDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Users>().Wait();
        }

        public Task<List<Users>> GetUsersAsync()
        {
            return database.Table<Users>().ToListAsync();
        }

        public Task<Users> GetUserAsync(int id)
        {
            return database.Table<Users>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(Users user)
        {
            if (user.Id != 0)
            {
                return database.UpdateAsync(user);
            }
            else
            {
                return database.InsertAsync(user);
            }
        }

        public Task<int> DeleteUserAsync(Users user)
        {
            return database.DeleteAsync(user);
        }
    }
}
