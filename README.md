#  Internal Project Management System

კომპანიის შიდა მართვის RESTful API, აწყობილი **ASP.NET Core 8**-ზე. სისტემა აერთიანებს კომპანიის პროექტების, თანამშრომლების, როლებისა და დავალებების მართვას. წვდომა კონტროლდება JWT ავთენტიფიკაციით და როლებით (`Admin`, `Manager`, `Employee`).

---

##  ფუნქციონალი

- **ავთენტიფიკაცია:** რეგისტრაცია, შესვლა, Access Token-ის განახლება Refresh Token-ით
- **როლები:** როლების შექმნა, რედაქტირება, წაშლა და მომხმარებლისთვის როლის მინიჭება
- **პროექტები:** პროექტების CRUD, წევრების დამატება და წაშლა
- **დავალებები:** დავალებების CRUD, მომხმარებელზე მიბმა, სტატუსის შეცვლა (`Pending` → `InProgress` → `Done`), ძებნა სახელით, პროექტით ან მომხმარებლით
- **ვალიდაცია:** შემავალი მონაცემები მოწმდება Data Annotations-ით
- **დოკუმენტაცია:** Swagger UI, JWT ავტორიზაციის მხარდაჭერით

---

##  ტექნოლოგიები

| სფერო | ტექნოლოგია |
|---|---|
| ფრეიმვორკი | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 (Database First) |
| მონაცემთა ბაზა | SQL Server |
| ავთენტიფიკაცია | JWT Bearer + Refresh Token |
| პაროლის ჰეშირება | BCrypt.Net-Next |
| მეპინგი | AutoMapper |
| ვალიდაცია | Data Annotations |
| დოკუმენტაცია | Swagger (Swashbuckle) |

---

##  პროექტის სტრუქტურა

```
internal-project-management-system/
├── final_task.sln
└── final_task/
    ├── Controllers/        # AuthController, RoleController, ProjectController, TasksController
    ├── Services/           # ბიზნეს-ლოგიკა (AuthService, RoleService, ProjectService, TasksProjectService)
    ├── Interfaces/         # სერვისების ინტერფეისები
    ├── Models/             # EF Core ენთითები და UsersContext
    ├── DTOs/               # Request/Response DTO-ები (Auth, Role, Project, Task)
    ├── TokensDto/          # TokenDto (Access + Refresh Token)
    ├── AutoMappers/        # AutoMapper-ის პროფილი
    ├── Program.cs          # DI, JWT, Swagger-ის კონფიგურაცია
    └── appsettings.json
```

---

##  მონაცემთა მოდელი

| ცხრილი | ველები |
|---|---|
| **Users** | `ID`, `EMAIL` (უნიკალური), `PASSWORD_HASH`, `FIRST_NAME`, `LAST_NAME`, `IS_ACTIVE`, `CREATED_AT`, `RefreshToken`, `RefreshTokenExpiry` |
| **Roles** | `ID`, `NAME` (უნიკალური), `DESCRIPTION`, `CREATED_AT` |
| **UsersRole** | `USERS_ID`, `ROLE_ID`, `ASSIGNED_AR` — მომხმარებელი ↔ როლი (many-to-many) |
| **Projects** | `Id`, `Name`, `Description`, `CreatedAt`, `Deadline` |
| **ProjectMembers** | `ProjectId`, `UserId`, `JoinedAt` — პროექტი ↔ მომხმარებელი (many-to-many) |
| **Tasks** | `Id`, `Name`, `Description`, `Deadline`, `Status`, `UserId` (nullable), `ProjectId` |

```
Users ──< UsersRole >── Roles
  │
  ├──< ProjectMembers >── Projects
  │                          │
  └──────────< Tasks >───────┘
```

მომხმარებლის წაშლისას მისი დავალებები არ იშლება: `UserId` ხდება `NULL`.

---

##  გაშვება

### საჭიროა
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (ან SQL Server Express / LocalDB)

### 1. კლონირება

```bash
git clone https://github.com/sabakiladze/internal-project-management-system.git
cd internal-project-management-system
```

### 2. Connection string-ის შეცვლა

`final_task/appsettings.json`-ში ჩაწერე შენი SQL Server-ის მისამართი:

```json
"ConnectionStrings": {
  "Default": "Server=YOUR_SERVER;Database=Users;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

>  **მნიშვნელოვანია:** `Models/UsersContext.cs`-ის `OnConfiguring` მეთოდში connection string პირდაპირ კოდშია ჩაწერილი (`Server=SABAKILA`) და appsettings-ის მნიშვნელობას ფარავს. ან იქაც შეცვალე, ან (უკეთესი გზა) მთლიანად წაშალე `OnConfiguring`, რომ კავშირი მხოლოდ `appsettings.json`-დან მოდიოდეს.

### 3. ბაზის შექმნა

პროექტი Database First მიდგომით შეიქმნა და მიგრაციები რეპოზიტორიაში არ არის. ბაზის შექმნა არსებული მოდელიდანაც შეიძლება:

```bash
dotnet tool install --global dotnet-ef

cd final_task
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. როლების და პირველი ადმინის დამატება

რეგისტრაციისას მომხმარებელი ავტომატურად იღებს `Employee` როლს, **თუ ასეთი როლი ბაზაში არსებობს**. როლის შექმნა და მინიჭება მხოლოდ ადმინს შეუძლია, ამიტომ საწყისი როლები და პირველი ადმინი SQL-ით უნდა დაამატო:

```sql
INSERT INTO Roles (NAME, DESCRIPTION) VALUES
  ('Admin',    'System administrator'),
  ('Manager',  'Project manager'),
  ('Employee', 'Regular employee');
```

შემდეგ დარეგისტრირდი API-ით (`POST /api/Auth/register`) და შენს ანგარიშს Admin როლი მიანიჭე:

```sql
INSERT INTO UsersRole (USERS_ID, ROLE_ID)
SELECT u.ID, r.ID
FROM Users u, Roles r
WHERE u.EMAIL = 'your@email.com' AND r.NAME = 'Admin';
```

როლი ტოკენში ჩაიწერება შემდეგი შესვლისას, ამიტომ ამის შემდეგ თავიდან შედი.

### 5. გაშვება

```bash
cd final_task
dotnet run --launch-profile https
```

Swagger UI: **https://localhost:7030/swagger** (ან http://localhost:5071/swagger)

---

##  კონფიგურაცია

| გასაღები | აღწერა |
|---|---|
| `ConnectionStrings:Default` | SQL Server-ის connection string |
| `Jwt:Secret` | ტოკენის ხელმოწერის გასაღები (**მინიმუმ 32 სიმბოლო**) |
| `Jwt:Issuer` | ტოკენის გამცემი (`ManagmentSystem`) |
| `Jwt:Audience` | ტოკენის ადრესატი (`ManagementSystemUsers`) |

რეპოზიტორიაში შენახული `Jwt:Secret` მხოლოდ მაგალითია. რეალური გასაღები user-secrets-ში შეინახე:

```bash
cd final_task
dotnet user-secrets init
dotnet user-secrets set "Jwt:Secret" "<მინიმუმ-32-სიმბოლოიანი-შემთხვევითი-გასაღები>"
```

---

##  ავთენტიფიკაცია

| ტოკენი | ვადა |
|---|---|
| Access Token (JWT) | 8 საათი |
| Refresh Token | 7 დღე (ყოველ განახლებაზე ახალი გაიცემა) |

JWT შეიცავს მომხმარებლის `Id`-ს, `Email`-ს და ყველა როლს.

**Swagger-ში ავტორიზაცია:** შესვლის შემდეგ დააკოპირე `accessToken`, დააჭირე **Authorize** ღილაკს და ჩასვი ტოკენი.

### რეგისტრაცია — მოთხოვნის მაგალითი

```json
POST /api/Auth/register
{
  "email": "giorgi@company.ge",
  "password": "Password1",
  "firstName": "Giorgi",
  "lastName": "Beridze"
}
```

პაროლი უნდა შეიცავდეს მინიმუმ ერთ დიდ ასოს და ერთ ციფრს.

### შესვლა — პასუხის მაგალითი

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "q8Yt3k...",
  "expiresAt": "2026-11-16T01:00:00Z"
}
```

### ტოკენის განახლება

`POST /api/Auth/refresh` იღებს Refresh Token-ს JSON სტრიქონის სახით:

```json
"q8Yt3k..."
```

---

## 📡 API Endpoint-ები

###  Auth — `/api/Auth`

| მეთოდი | მისამართი | აღწერა | წვდომა |
|---|---|---|---|
| POST | `/api/Auth/register` | რეგისტრაცია | ყველა |
| POST | `/api/Auth/login` | შესვლა, აბრუნებს Access და Refresh Token-ს | ყველა |
| POST | `/api/Auth/refresh` | Access Token-ის განახლება | ყველა |

###  Roles — `/api/Role`

| მეთოდი | მისამართი | აღწერა | წვდომა |
|---|---|---|---|
| GET | `/api/Role` | ყველა როლი | ავტორიზებული |
| GET | `/api/Role/{id}` | როლი ID-ით | ავტორიზებული |
| POST | `/api/Role` | როლის შექმნა | Admin |
| PUT | `/api/Role/{id}` | როლის რედაქტირება | Admin |
| DELETE | `/api/Role/{id}` | როლის წაშლა | Admin |
| POST | `/api/Role/addroletouser/{userId}/{roleId}` | მომხმარებლისთვის როლის მინიჭება | Admin, Manager |

###  Projects — `/api/Project`

| მეთოდი | მისამართი | აღწერა | წვდომა |
|---|---|---|---|
| GET | `/api/Project` | ყველა პროექტი | Admin, Manager |
| GET | `/api/Project/{id}` | პროექტი ID-ით | Admin, Manager |
| GET | `/api/Project/{projectId}/members` | პროექტის წევრები | Admin, Manager |
| POST | `/api/Project` | პროექტის შექმნა | Admin, Manager |
| PUT | `/api/Project/{id}` | პროექტის რედაქტირება | Admin, Manager |
| DELETE | `/api/Project/{id}` | პროექტის წაშლა | Admin |
| POST | `/api/Project/{projectId}/members/{userId}` | წევრის დამატება | Admin, Manager |
| DELETE | `/api/Project/{projectId}/members/{userId}` | წევრის წაშლა | Admin, Manager |

###  Tasks — `/api/Tasks`

| მეთოდი | მისამართი | აღწერა | წვდომა |
|---|---|---|---|
| GET | `/api/Tasks` | ყველა დავალება | Admin, Manager |
| GET | `/api/Tasks/{id}` | დავალება ID-ით | ავტორიზებული |
| GET | `/api/Tasks/by-name/{name}` | დავალებები სახელით | ავტორიზებული |
| GET | `/api/Tasks/by-project/{projectId}` | პროექტის დავალებები | ავტორიზებული |
| GET | `/api/Tasks/by-user/{userId}` | მომხმარებლის დავალებები | ავტორიზებული |
| POST | `/api/Tasks` | დავალების შექმნა | Admin, Manager |
| PUT | `/api/Tasks/{id}` | დავალების რედაქტირება | ავტორიზებული |
| PUT | `/api/Tasks/{taskId}/assign/{userId}` | მომხმარებლის მიბმა | ავტორიზებული |
| DELETE | `/api/Tasks/{taskId}/remove-user` | მომხმარებლის მოხსნა | ავტორიზებული |
| PUT | `/api/Tasks/{taskId}/status?status=Done` | სტატუსის შეცვლა | ავტორიზებული |
| DELETE | `/api/Tasks/{id}` | დავალების წაშლა | ავტორიზებული |

**დავალების სტატუსები:** `Pending`, `InProgress`, `Done`

### დავალების შექმნა — მაგალითი

```json
POST /api/Tasks
{
  "name": "Login გვერდის დიზაინი",
  "description": "ავტორიზაციის გვერდის UI-ის მომზადება",
  "deadline": "2026-12-01T18:00:00",
  "status": "Pending",
  "userId": null,
  "projectId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

##  სამომავლო გეგმები

- [ ] `UserController`: მომხმარებლების სია, როლის მოხსნა, დეაქტივაცია
- [ ] `GET /api/Project/my-projects` და `GET /api/Tasks/my-tasks` მიმდინარე მომხმარებლისთვის
- [ ] Tasks-ის რედაქტირების, მიბმისა და წაშლის endpoint-ებზე როლების შეზღუდვა (Admin / Manager)
- [ ] გლობალური შეცდომების დამუშავება (middleware). ახლა, მაგალითად, არასწორ ლოგინზე 401-ის ნაცვლად 500 ბრუნდება
- [ ] დეაქტივირებული (`IS_ACTIVE = false`) მომხმარებლის შესვლის აკრძალვა
- [ ] ბაზის მიგრაციები და საწყისი როლების ავტომატური seed
- [ ] Unit ტესტები (xUnit)
- [ ] Connection string-ის ამოღება `UsersContext.OnConfiguring`-დან და საიდუმლოების user-secrets-ში გადატანა

---

## 👤 ავტორი

**საბა კილაძე** — [@sabakiladze](https://github.com/sabakiladze)
