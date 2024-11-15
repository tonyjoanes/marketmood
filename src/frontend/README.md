# MarketMood Project

## Project Structure

```
src/frontend/
├── src/
│   ├── app/
│   │   ├── layout.tsx (minimal shell)
│   │   ├── page.tsx (landing page)
│   │   └── admin/
│   │       ├── layout.tsx (admin-specific layout)
│   │       ├── page.tsx (admin dashboard)
│   │       ├── products/
│   │       │   └── page.tsx
│   │       ├── analytics/
│   │       │   └── page.tsx
│   │       └── settings/
│   │           └── page.tsx
│   └── components/
│       └── admin/
│           └── AdminLayout.tsx (main admin layout component)
```

## Features Implemented

### Admin Dashboard
- Responsive layout with sidebar navigation
- Active state highlighting in navigation
- Dashboard overview with stats cards
- Products management page with table view
- Analytics page with placeholder charts
- Settings page with configuration options

### Navigation Structure
- Dashboard (`/admin`)
- Products (`/admin/products`)
- Analytics (`/admin/analytics`)
- Settings (`/admin/settings`)

### Design Decisions
- Using Next.js 14 with App Router
- Tailwind CSS for styling
- Lucide icons for UI elements
- Client-side navigation with active state highlighting
- Separate layouts for admin and customer areas

## Planned Features
- Customer-facing product search and review insights
- Authentication for admin area
- Integration with backend API
- Analytics visualization
- Product review management

## Configuration
- Using TypeScript for type safety
- Next.js standalone output for Docker deployment
- Environment variables for API configuration

## Docker Setup
```yaml
services:
  frontend:
    build:
      context: ../
      dockerfile: docker/Dockerfile.frontend
    ports:
      - "3000:3000"
    environment:
      - NEXT_PUBLIC_API_URL=http://product-review-api:8080
```

## Development
```bash
# Start development server
npm run dev

# Build for production
npm run build

# Start production server
npm run start
```

## Key Dependencies
- Next.js 14
- React 18
- Tailwind CSS
- Lucide Icons
```

This README provides:
1. Current project structure
2. Features implemented
3. Design decisions made
4. Planned features
5. Configuration details
6. Development commands

You can keep this updated as we add more features. When starting a new conversation, you can share this README to get me up to speed quickly. Would you like me to:

1. Add more specific technical details to the README?
2. Create additional documentation for specific features?
3. Add a contributing guide?
4. Create a development setup guide?

Let me know what additional documentation would be helpful!

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.
