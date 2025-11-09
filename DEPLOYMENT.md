# 🚀 Deployment Guide - HotelHalo Hub Backend

## Deploy to Render

### Option 1: Using Render Dashboard (Recommended)

1. **Push to GitHub**
   ```bash
   git add .
   git commit -m "Prepare for Render deployment"
   git push origin main
   ```

2. **Create New Web Service on Render**
   - Go to [Render Dashboard](https://dashboard.render.com/)
   - Click "New +" → "Web Service"
   - Connect your GitHub repository: `hotelhalo-hub`

3. **Configure Service**
   - **Name**: `hotelhalo-api` (or your choice)
   - **Environment**: `Docker`
   - **Region**: Choose closest to you
   - **Branch**: `main`
   - **Plan**: `Free`

4. **Deploy**
   - Click "Create Web Service"
   - Render will automatically detect the Dockerfile and build
   - Wait for deployment (takes 5-10 minutes first time)

5. **Get Your API URL**
   - After deployment, you'll get a URL like: `https://hotelhalo-api.onrender.com`
   - Test it: `https://hotelhalo-api.onrender.com/api/rooms`

### Option 2: Using render.yaml

The repository includes a `render.yaml` file for automatic deployment:

1. Push code to GitHub
2. On Render: Click "New +" → "Blueprint"
3. Connect repository
4. Render will auto-configure from `render.yaml`

---

## Important Notes

### ⚠️ Free Tier Limitations
- **Render Free Tier** spins down after 15 minutes of inactivity
- **First request** after spin-down takes ~30-60 seconds (cold start)
- **In-memory data** is lost when service restarts
- Perfect for demos and portfolios!

### 🔄 Data Persistence
This API uses **in-memory storage**:
- ✅ Data persists during active session
- ✅ Data survives page refreshes
- ❌ Data resets when Render restarts the service
- ❌ Data lost when service spins down (after 15 min inactivity)

---

## After Deployment

### Update Frontend
Once deployed, update your frontend (`hotelhalo-hub-fe`) to use the Render API URL:

**Create `.env` file in frontend:**
```env
VITE_API_URL=https://your-app.onrender.com/api
```

**Update API calls to use:**
```typescript
const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5188/api';
```

### Test Your API
```bash
# Get all rooms
curl https://your-app.onrender.com/api/rooms

# Get dashboard stats
curl https://your-app.onrender.com/api/dashboard/stats

# Swagger UI
https://your-app.onrender.com/swagger
```

---

## Troubleshooting

### Service won't start
- Check Render logs in dashboard
- Verify Dockerfile is in repository root
- Ensure all files are committed

### CORS errors
- Verify your Vercel URL is allowed
- Check browser console for exact error
- Frontend URL must match CORS settings in Program.cs

### Port issues
- Render automatically sets `$PORT` environment variable
- Dockerfile is configured to use it

---

## Local Development

Run locally:
```bash
cd server/HotelHaloHub.API
dotnet run
```

API will be available at: `http://localhost:5188`

---

## Support

- [Render Docs](https://render.com/docs)
- [.NET Docker Docs](https://learn.microsoft.com/en-us/dotnet/core/docker/introduction)
