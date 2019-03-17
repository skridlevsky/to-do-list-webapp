using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data;
using TodoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns array of items
        /// </summary>
        /// <returns>To-do list items</returns>
        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
        {
            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == user.Id)
                .ToArrayAsync();
        }

        /// <summary>
        /// Returns array of items
        /// </summary>
        /// <returns>To-do list items</returns>
        public async Task<TodoItem[]> GetCompleteItemsAsync(IdentityUser user)
        {
            return await _context.Items
                .Where(x => x.IsDone == true && x.UserId == user.Id)
                .ToArrayAsync();
        }

        /// <summary>
        /// An action that handles adding new items to the to-do list
        /// </summary>
        /// <param name="newItem">New item that is being added to the list</param>
        /// <returns>Success / failure</returns>
        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.UserId = user.Id;

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        /// <summary>
        /// An action that handles checking an item as "completed"
        /// </summary>
        /// <param name="id">GUID of checked item</param>
        /// <returns>Success / failure</returns>
        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}