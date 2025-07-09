# ChatBot

This repository contains a prototype AI chatbot consisting of an ASP.NET Core backend and an Angular frontend.

## Prerequisites
- .NET 8 SDK
- Node.js 20 with npm
- SQL Server instance (localdb is sufficient)

## Running the backend
1. Navigate to `ChatBotApi`:
   ```bash
   cd ChatBotApi
   ```
2. Adjust the connection string in `appsettings.json` if needed. If you receive a
   login error complaining about an untrusted certificate, append
   `TrustServerCertificate=True` to the connection string.
3. Run the API:
   ```bash
   dotnet run
   ```
   The API returns listenig ports after run `ex. Now listening on: https://localhost:7201` . (Prod eviroment will improve it)
   CORS is configured to allow requests from `http://localhost:4200`.
   The database schema is created automatically on first run.
   
## Running the frontend
1. Navigate to `ChatBotUi`:
   ```bash
   cd ChatBotUi
   ```
2. Install dependencies and start the dev server:
   ```bash
   npm install
   npm start
   ```
   If UI cant connect to API, please use proxy.conf.json with port returned by API console.
   The application will be available at `http://localhost:4200`.
   
## Testing
- Visit the application in Chrome, Firefox or Safari and send a few messages. The bot replies with randomly generated text.
- Use the 👍/👎 buttons to rate bot messages.

Integration between frontend and backend can be tested by running both servers locally and verifying that messages and ratings persist across refreshes.