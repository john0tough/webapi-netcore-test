using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI1.Models;

namespace NetCoreAPI1.Controllers
{

   /// <summary>
   /// TodoController 
   /// </summary>
   // [Produces("application/json")] // define el tipo de salida
   [Consumes("application/json")] // deine el tipo de entrada
   [Route("api/[controller]")] //establece el prefijo de entrada del controllador
   [ApiController]
   public class TodoController : ControllerBase
   {
      private readonly TodoContext _context;
      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="context"></param>
      public TodoController(TodoContext context)
      {
         this._context = context;
         if (!this._context.TodoItems.Any())
         {
            this._context.TodoItems.Add(new TodoItem { isComplete = false, name = "Todo 1" });
            this._context.SaveChanges();
         }
      }

      // Get api/todo
      /// <summary>
      /// get all todo items
      /// </summary>
      /// <returns>List</returns>
      [HttpGet]
      public async Task<List<TodoItem>> GetTodoItems() // respuesta de tipo especifico
      {
         return await _context.TodoItems.ToListAsync();
      }

      // Get api/todo/5
      /// <summary>
      /// get single todo item
      /// </summary>
      /// <param name="id">id todo item</param>
      /// <returns>IActionResult</returns>
      /// <response code="201">found object</response>
      /// <response code="404">not found object</response>
      [HttpGet("{id}")]
      [ProducesResponseType(201, Type = typeof(TodoItem))]
      [ProducesResponseType(404)]
      public async Task<IActionResult> GetTodoItem(int id) // Type IActionResult, defines Type in ProducesResponseType
      {
         var todoItem = await this._context.TodoItems.FindAsync(id);

         if (todoItem == null)
         {
            return NotFound();
         }

         return Ok(todoItem);
      }

      // POST: api/Todo
      /// <summary>
      /// creates a new todo item
      /// </summary>
      /// <param name="item"></param>
      /// <returns>ActionResult</returns>
      /// <response code="201">todo item created</response>
      /// <response code="400">todo item not created, model not valid</response>
      [HttpPost]
      [ProducesResponseType(201)]
      [ProducesResponseType(400)]
      public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item) //tipo ActionResult<T>
      {
         if (item.name != string.Empty) // si el modelo corresponde devolvera 201
         {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
         }

         return BadRequest(); // si el modelo no corresponde se devuelve 400
      }
      // PUT: api/Todo/5
      /// <summary>
      /// updates a todo item
      /// </summary>
      /// <param name="id">id todo item</param>
      /// <param name="item">params to update</param>
      /// <returns>IActionResult</returns>
      /// <response code="204">actualizado correctamente</response>
      /// <response code="400">bad request</response>
      [HttpPut("{id}")]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
      {
         if (id != item.Id)
         {
            return BadRequest();
         }

         _context.Entry(item).State = EntityState.Modified;
         await _context.SaveChangesAsync();
         return NoContent();
      }
      // DELETE: api/Todo/5
      /// <summary>
      /// delete a todo item
      /// </summary>
      /// <param name="id">id todo item</param>
      /// <returns>ActionResult</returns>
      /// <response code="204">Correctly Deletes </response>
      /// <response code="400">bad request</response>
      [HttpDelete("{id}")]
      [ProducesResponseType(204)]
      [ProducesResponseType(400)]
      public async Task<ActionResult> DeleteTodoItem(long id)
      {
         var todoItem = await _context.TodoItems.FindAsync(id);

         if (todoItem == null)
         {
            return NotFound();
         }

         _context.TodoItems.Remove(todoItem);
         await _context.SaveChangesAsync();

         return NoContent();
      }
   }
}