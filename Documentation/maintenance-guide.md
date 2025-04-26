# Student Term Tracker - Maintenance Guide

## Table of Contents
1. [System Requirements](#system-requirements)
2. [Development Environment Setup](#development-environment-setup)
3. [Database Management](#database-management)
4. [Application Maintenance](#application-maintenance)
5. [Troubleshooting](#troubleshooting)
6. [Backup Procedures](#backup-procedures)

## System Requirements

### Hardware Requirements
- Windows 10 or later
- Minimum 4GB RAM
- 2GB free disk space
- Internet connection for API functionality

### Software Requirements
- Visual Studio 2022 or later
- .NET 8.0 SDK or later
- Azure SQL Database access
- Git for version control

## Development Environment Setup

### 1. Clone the Repository
```powershell
git clone https://gitlab.com/wgu-gitlab-environment/student-repos/bbail39/d424-software-engineering-capstone.git
cd WGU_D424_SWE_Capstone
```

### 2. Restore NuGet Packages
- Open the solution in Visual Studio
- Right-click on the solution in Solution Explorer
- Select "Restore NuGet Packages"

### 3. Database Setup
1. Ensure you have access to the Azure SQL Database
2. Update connection strings in `appsettings.json` with your Azure SQL connection string
3. Verify database schema matches the application requirements

### 4. Build and Run
1. Build the solution:
   ```powershell
   dotnet build
   ```
2. Run the application:
   ```powershell
   dotnet run --project StudentTermTracker
   ```

## Database Management

### Azure SQL Database Management
1. Access the Azure Portal (portal.azure.com)
2. Navigate to your SQL Database resource
3. Use the Query Editor or Azure Data Studio for database operations

### Database Maintenance Tasks
- Monitor database performance through Azure Portal
- Review and optimize query performance
- Check for long-running queries
- Monitor database size and growth

## Application Maintenance

### Regular Maintenance Tasks
1. **Daily**
   - Check application logs for errors
   - Verify Azure SQL Database connectivity
   - Monitor application performance
   - Review Azure SQL Database metrics

2. **Weekly**
   - Review error logs
   - Check for pending updates
   - Verify backup integrity
   - Review query performance

3. **Monthly**
   - Update dependencies
   - Review and optimize database performance
   - Test backup restoration
   - Review Azure SQL Database costs and usage

### Code Updates
1. Create a new branch for maintenance work
2. Implement changes
3. Run unit tests
4. Create pull request
5. Deploy after approval

## Troubleshooting

### Common Issues

#### Azure SQL Database Connection Issues
1. Verify Azure SQL Database is running and accessible
2. Check connection strings in `appsettings.json`
3. Ensure proper Azure SQL Database permissions
4. Verify network connectivity to Azure

#### Application Errors
1. Check application logs
2. Verify all services are running
3. Check for missing dependencies
4. Review Dapper query execution logs

### Log Files
- Application logs are located in: `StudentTermTracker/logs`
- Azure SQL Database logs are accessible through the Azure Portal

## Backup Procedures

### Azure SQL Database Backup
1. Azure SQL Database automatically performs backups:
   - Point-in-time restore (PITR) backups every 5-10 minutes
   - Weekly full backups
   - Monthly differential backups

2. Manual backup procedures:
   ```sql
   -- Create a database copy
   CREATE DATABASE [StudentTermTracker_Copy] AS COPY OF [StudentTermTracker]
   ```

### Restore Procedures
1. Use Azure Portal to restore from a point-in-time backup
2. Deploy application from source control
3. Verify all functionality

## Contact Information
For maintenance support, contact:
- System Administrator: [Contact Information]
- Azure Database Administrator: [Contact Information]
- Development Team: [Contact Information]

---

*Last Updated: [Current Date]*
*Version: 1.0* 