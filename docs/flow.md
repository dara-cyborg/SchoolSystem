Here's a full presentation plan you can run with your junior devs, structured as a walkthrough session — not just a dump of docs.

---

# SchoolSystem — Dev Handoff Presentation Plan

## Format suggestion
**2–3 hours, one session.** Split into two halves with a break in the middle. Have Postman and the running API open on a shared screen the whole time. The API_REFERENCE.md stays open in a second window as a reference they can follow along with.

---

## Part 1 — Architecture & Foundation (~60 min)

### Block 1: The Big Picture (15 min)

Start here before touching any code. Draw or show a simple diagram of the 4 projects and how they relate:

- **SchoolSystem.Core** — shared models, DTOs, interfaces, enums. No code runs here, it's just contracts and data shapes.
- **SchoolSystem.Api** — the REST API your juniors will be working with. Services live here, controllers live here, it talks to PostgreSQL via EF Core.
- **SchoolSystem.Web** — Razor Pages parent portal (T19–T22, not built yet). Uses cookie auth, not JWT.
- **SchoolSystem.Desktop** — WinForms app for teachers/admins (T13–T18, not built yet). Uses JWT same as the API.

Emphasize the key architectural decision: **no repository pattern**. Services inject `AppDbContext` directly. This keeps it simple — if they see a service, they'll see the database queries right there in the same file.

### Block 2: The Database & Domain Models (15 min)

Walk through the 21 entities. Don't go line by line — pick the most important relationships and draw them out:

- `User` → `UserRole` → `Role` (a user can be teacher AND homeroom at once)
- `Grade` → `Class` → `ClassSubject` → `Subject` + `User` (teacher assignment happens here)
- `Class` → `Student` → `ParentStudent` → `User` (parent linkage)
- The score chain: `GradebookEntry` → `MonthlyScore` → `SemesterScore` → `YearlyScore`
- The report chain: `MonthlyReport` → `SemesterReport` → `YearlyReport` → `Feedback`

The key point to hammer: **ClassSubject is the pivot of everything.** Attendance, gradebook entries, and scores are all scoped to a ClassSubject, not just a Class or Subject alone. This is what enforces which teacher owns what.

### Block 3: Authentication — How JWT Works Here (15 min)

Show the flow live in Postman:

1. Call `POST /api/auth/login` → get back a token
2. Show what's inside the token (use jwt.io to decode it) — point out the `role` claim and `sub` (userId)
3. Show what happens when you call a protected endpoint without the token → 401
4. Show what happens when you use a token with the wrong role → 403

Then explain how the API uses the token internally: `User.GetUserId()` extracts the userId from claims. This is how the teacher ownership check works — the API doesn't trust the request body to say who the teacher is; it reads directly from the JWT.

Roles to cover:
- `super_admin` — full access, manages users and academic structure
- `teacher` — attendance + gradebook, but only for *their* ClassSubjects
- `homeroom` — read gradebook, edit unlocked scores, submit reports
- `parent` — read-only, their children only
- One user can have multiple roles

### Block 4: Project Structure — Where Things Live (15 min)

Open the solution in the editor and walk through the folder structure physically:

```
SchoolSystem.Core/
  Constants/Enums.cs       ← all enums (RoleName, AttendanceStatus, ReportType, SexType)
  Models/                  ← 21 entity classes
  DTOs/                    ← what goes in/out of the API
  Interfaces/              ← service contracts
  Extensions/              ← ClaimsPrincipalExtensions (GetUserId, HasRole)

SchoolSystem.Api/
  Data/AppDbContext.cs     ← single EF Core context, all 21 DbSets
  Services/                ← business logic, one service per feature
  Controllers/             ← HTTP layer, thin — no business logic here
  Program.cs               ← DI registrations, middleware, JWT config
```

Point out the pattern explicitly: **Interface in Core → Service in Api → Controller in Api.** Every feature follows this same shape.

---

## Break (10 min)

---

## Part 2 — API Walkthrough & Live Usage (~70 min)

This is the hands-on half. Every junior dev should have Postman open and be making requests themselves, not just watching.

### Block 5: Setup (5 min)

Hand out or share the Postman collection. Have everyone:
1. Set the base URL variable
2. Log in and copy their token into the collection variable
3. Verify they can hit `GET /api/users` successfully as super_admin

### Block 6: Academic Structure — How Data Is Set Up (15 min)

Walk through the dependency chain of setup data, because nothing else works until this exists:

1. Create a Grade (`POST /api/grades`)
2. Create a Class under that Grade (`POST /api/classes`)
3. Create a Subject (`POST /api/subjects`)
4. Create a ClassSubject linking the Class + Subject + Teacher (`POST /api/class-subjects`)
5. Create a Student and assign to the Class (`POST /api/students`)
6. Link a Parent to the Student (`POST /api/students/{id}/link-parent`)

Let them do this themselves with made-up data. This gives them a working dataset for all subsequent blocks.

### Block 7: Attendance (10 min)

Switch to a teacher token. Show:
- `POST /api/attendance` — single record
- `POST /api/attendance/bulk` — multiple students at once
- Try marking attendance for a ClassSubject that doesn't belong to this teacher → should get `400` with a clear message
- `GET /api/attendance/summary/{studentId}?month=&schoolYear=` — show the absence count

Emphasize the ownership rule: the API extracts the teacher's userId from the JWT and checks it against `ClassSubject.TeacherUserId`. The teacher cannot spoof this by sending a different ClassSubjectId.

### Block 8: Gradebook (10 min)

Still as teacher:
- `POST /api/gradebook` — create a few entries (label = "Quiz 1", score, maxScore)
- `GET /api/gradebook/class-subject/{id}` — see all entries for the class
- `GET /api/gradebook/student/{id}/class-subject/{id}` — filter to one student
- `PUT /api/gradebook/{id}` — update label/score
- `DELETE /api/gradebook/{id}`

Switch to a homeroom token and show that GET works but POST/PUT/DELETE return 403.

Clarify what gradebook entries are: **teacher working notes, not final scores.** They don't affect reports. Monthly scores are separate.

### Block 9: The Score & Report Lifecycle (20 min)

This is the most important and most complex part. Walk through it as a story:

**Step 1 — Teacher submits monthly scores** (`POST /api/monthly-scores/submit`)
- One final score per student per ClassSubject per month
- Upsert: calling it again updates the existing score
- Scores start unlocked (`isLocked = false`)

**Step 2 — Homeroom edits if needed** (`PUT /api/monthly-scores/{id}`)
- Only works while `isLocked = false`
- Show what happens if you try to edit a locked score → `400: "Score is locked and cannot be edited."`

**Step 3 — Homeroom submits monthly report** (`POST /api/monthly-reports/submit`)
- This locks all scores for that class+month+year
- Computes rank (sum of all subject scores per student, ordered DESC)
- Try submitting the same report twice → `400`

**Step 4 — After 6 months** (`POST /api/semester-reports/submit`)
- Averages the 6 monthly scores per subject
- Requires all monthly reports already submitted
- Show the 400 you get if you try it without all months locked

**Step 5 — After both semesters** (`POST /api/yearly-reports/submit`)
- Requires both semester reports

**Step 6 — Parent reads the report** (`GET /api/monthly-reports/{id}`)
- Switch to parent token, show they can read it
- Try to read a report for a class whose student is NOT linked to this parent → 403

### Block 10: Feedback (10 min)

As parent:
- `POST /api/feedback` — submit feedback on a monthly report (show the `reportType` + `reportId` pattern)
- Try submitting feedback for another student's report → 400
- `GET /api/feedback/report/monthly/{id}` — read feedback

As homeroom:
- `GET /api/feedback/class/{classId}` — see all feedback across all report types for the class

---

## Part 3 — What They're Building (10 min)

Close with a clear map of what's left:

| Task | Who | What |
|---|---|---|
| T13–T18 | Member B | WinForms desktop app — ApiClient, login, all forms |
| T19–T22 | Member C | Razor Pages web portal — cookie auth, parent dashboard, reports, feedback |

Point them to:
- `API_REFERENCE.md` — everything they need to know about calling the API
- The Postman collection — working examples for every endpoint
- `AppDbContext.cs` — if they ever need to understand the data shape behind an endpoint

End with one clear rule to set expectations: **the API is done and stable. They do not modify any T01–T12 files. If they think something in the API needs to change, they come to you first.**

---

## Materials to prepare before the session

1. Running API instance (local or dev server) everyone can hit
2. Postman collection imported and shared (the one generated from T09–T12 prompts)
3. A seeded database with at least one user per role
4. `API_REFERENCE.md` open and bookmarked
5. jwt.io open in a browser tab for the JWT decode demo