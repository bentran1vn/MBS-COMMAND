# Mentor Booking System (MBS)


## 0. Introduction

A platform designed to connect students with mentors, facilitating skill development, project support, and guidance through structured mentorship. The system is tailored to meet the needs of three key user roles: **Students**, **Mentors**, and **Admins**, each with specific functions to ensure a seamless mentoring experience.

## 1. User Roles and Features

- **Students**: 
  - Manage project groups and assign mentors based on skills.
  - Search for mentors with specific expertise.
  - View profile and performance points.
  - Book mentorship sessions for project guidance.

- **Mentors**: 
  - Showcase skills and certifications.
  - Manage schedule and verify student bookings.
  - Verify and manage group requests.

- **Admins**: 
  - Configure student points and approve mentors.
  - Manage mentor profiles and oversee mentor-student interactions.
  - View feedback from mentoring sessions to ensure quality.

## 2. Tech Stack

### Backend
- ASP.NET Core 8
- EF Core
- MediatR
- RabbitMQ
- MassTransit

### Frontend
- ReactJS, TypeScript
- Tailwind CSS
- Next.js

### Mobile
- Java for Android

### Deployment

- **Docker** for containerization, enabling consistent deployment across environments.
- **GitHub Workflow** and **GitHub Runner** for a streamlined CI/CD pipeline, automating the build and deployment process.
- **Nginx** for domain management and port forwarding.
- **YARP Reverse Proxy** for efficient request forwarding to specific backend services, enhancing load management.

### Database

- **MSSQL Server**: Ensures data consistency and adheres to ACID principles for transactional integrity.
- **MongoDB**: Optimized for fast read-heavy queries.
- **Redis**: Used for caching frequently accessed data and token management, improving response times.

### Architecture

- **Clean Architecture**: Modular and maintainable, facilitating easy scalability and testing.

## 3. System Standout Features

- **CQRS Pattern**: Separates read and write operations, with **MongoDB** dedicated to reads and **MSSQL Server** to writes.
- **Repository Pattern**: Provides abstraction for reusable and testable data access.
- **Outbox Pattern**: Ensures reliable event handling by persisting events before processing.
- **Idempotent Pattern**: Manages duplicate events to prevent redundant actions, ensuring message consistency.

---

This system architecture and tech stack ensure that the Mentor Booking System (MBS) is scalable, efficient, and maintains a high-quality user experience across all roles.
