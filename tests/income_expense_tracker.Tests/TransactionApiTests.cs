
using global::income_expense_tracker.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;


namespace income_expense_tracker.tests.income_expense_tracker.Tests
{
    public class TransactionApiTests
    {
        [Fact]
        public async Task Complete_Transaction_CRUD_Works()
        {
            await using var factory =
                new WebApplicationFactory<Program>();

            // Create schema in the disposable CI database.
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<TransactionDbContext>();

                await db.Database.MigrateAsync();
            }

            using var client = factory.CreateClient();

            long date = DateTimeOffset.UtcNow
                .ToUnixTimeMilliseconds();

            var request = new
            {
                title = "CI Test Lunch",
                category = "FOOD",
                amount = 1500.00m,
                type = "EXPENSE",
                date,
                remark = "Created during integration test"
            };

            // 1. CREATE
            var create = await client.PostAsJsonAsync(
                "/api/transcation", request);

            Assert.Equal(
                HttpStatusCode.Created, create.StatusCode);

            var created = await create.Content
                .ReadFromJsonAsync<JsonElement>();

            long id = created.GetProperty("id").GetInt64();
            Assert.True(id > 0);

            // 2. GET BY ID
            var get = await client.GetAsync(
                $"/api/transcation/{id}");

            Assert.Equal(HttpStatusCode.OK, get.StatusCode);

            var fetched = await get.Content
                .ReadFromJsonAsync<JsonElement>();

            Assert.Equal(
                "Created during integration test",
                fetched.GetProperty("remarks").GetString()
            );

            // 3. GET ALL
            var getAll = await client.GetAsync(
                "/api/transcation");

            Assert.Equal(HttpStatusCode.OK, getAll.StatusCode);

            // 4. UPDATE
            var updateRequest = new
            {
                title = "Updated Lunch",
                category = "FOOD",
                amount = 2000.00m,
                type = "EXPENSE",
                date,
                remark = "Updated by CI"
            };

            var update = await client.PutAsJsonAsync(
                $"/api/transcation/{id}",
                updateRequest);

            Assert.Equal(HttpStatusCode.OK, update.StatusCode);

            var updated = await update.Content
                .ReadFromJsonAsync<JsonElement>();

            Assert.Equal(
                "Updated by CI",
                updated.GetProperty("remarks").GetString()
            );

            // 5. DELETE
            var delete = await client.DeleteAsync(
                $"/api/transcation/{id}");

            Assert.True(delete.IsSuccessStatusCode);

            // 6. CONFIRM RECORD WAS DELETED
            var afterDelete = await client.GetAsync(
                $"/api/transcation/{id}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                afterDelete.StatusCode);

            // 7. VALIDATION CHECK
            var invalid = new
            {
                title = "Invalid Amount",
                category = "FOOD",
                amount = -10.00m,
                type = "EXPENSE",
                date,
                remark = (string?)null
            };

            var invalidResponse = await client.PostAsJsonAsync(
                "/api/transcation", invalid);

            Assert.Equal(
                HttpStatusCode.BadRequest,
                invalidResponse.StatusCode);
        }
    }
}
