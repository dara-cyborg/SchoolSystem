# SchoolSystem API Reference

This API uses JWT Bearer authentication.

Send the token in the `Authorization` header on every request except `POST /api/auth/login`:

```http
Authorization: Bearer {jwt_token}
```

Roles in this system:

- `super_admin` has full system access.
- `teacher` manages attendance and gradebook entries for assigned class-subjects only.
- `homeroom` manages class rosters, edits scores before lock, and submits reports.
- `parent` has read-only access to linked children’s reports and can submit feedback.
- A single user can hold multiple roles.

Standard HTTP status codes used across the API:

- `200 OK` for successful GET or PUT.
- `201 Created` for successful POST that creates a resource.
- `204 No Content` for successful DELETE.
- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` for missing or invalid JWT.
- `403 Forbidden` for authenticated users without enough permission.
- `404 Not Found` for missing resources.

Unless stated otherwise, error responses use this shape:

```json
{ "message": "..." }
```

## Auth

### POST /api/auth/login

| Item | Details |
| --- | --- |
| Roles | No auth required |
| Description | Authenticates a user with username and password, then returns a JWT token for later requests. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | The user name used to sign in. |
| password | string | Yes | The user password. |

Response body shape:

```json
{ "token": "string" }
```

Possible errors:

- `401 Unauthorized` if the credentials are invalid.

## Users

### GET /api/users?page=1&pageSize=20

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns a paginated list of users, including their roles. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Page number. Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Number of items per page. Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### GET /api/users/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns one user by ID, including roles. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The user ID. |

Response body shape:

```json
{ "...": "user dto fields" }
```

Possible errors:

- `404 Not Found` if the user does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### POST /api/users

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Creates a new user and hashes the password server-side. Passwords are never stored as plain text. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | User display name. |
| sex | enum | Yes | `male` or `female`. |
| dob | ISO date | Yes | Date of birth. |
| contact | string | Yes | Contact detail such as phone or email, depending on app rules. |
| password | string | Yes | Plain password sent once for creation only. |
| roleIds | int[] | Yes | Role IDs to assign to the new user. |

Response body shape:

```json
{ "...": "created user dto fields" }
```

Possible errors:

- `400 Bad Request` for validation failures or invalid role IDs.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### PUT /api/users/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Updates an existing user except for the password. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Updated name. |
| sex | enum | Yes | `male` or `female`. |
| dob | ISO date | Yes | Updated date of birth. |
| contact | string | Yes | Updated contact detail. |
| isActive | bool | Yes | Enables or disables the user. |

Response body shape:

```json
{ "...": "updated user dto fields" }
```

Possible errors:

- `404 Not Found` if the user does not exist.
- `400 Bad Request` for validation failures.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### DELETE /api/users/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Deletes a user. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The user ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the user does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### POST /api/users/{id}/roles

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Replaces the user’s current roles with the provided role list. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The user ID. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| roleIds | int[] | Yes | The full new set of role IDs for the user. |

Response body shape:

```json
{ "...": "updated user dto fields" }
```

Possible errors:

- `400 Bad Request` if `roleIds` is empty or invalid.
- `404 Not Found` if the user does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Grades

### GET /api/grades

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns a paginated list of grades. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### GET /api/grades/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns one grade by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The grade ID. |

Response body shape:

```json
{ "...": "grade dto fields" }
```

Possible errors:

- `404 Not Found` if the grade does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### POST /api/grades

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Creates a new grade. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Grade name. |

Response body shape:

```json
{ "...": "created grade dto fields" }
```

Possible errors:

- `400 Bad Request` for validation failures.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### PUT /api/grades/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Updates an existing grade. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | New grade name. |

Response body shape:

```json
{ "...": "updated grade dto fields" }
```

Possible errors:

- `404 Not Found` if the grade does not exist.
- `400 Bad Request` for validation failures.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### DELETE /api/grades/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Deletes a grade. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The grade ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the grade does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Classes

### GET /api/classes?gradeId=&schoolYear=

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns a paginated list of classes. Both filters are optional and can be used together. Homeroom users can use the filters to narrow the list. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| gradeId | int? | No | Filter by grade. |
| schoolYear | short? | No | Filter by school year. |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/classes/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns one class by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The class ID. |

Response body shape:

```json
{ "...": "class dto fields" }
```

Possible errors:

- `404 Not Found` if the class does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/classes/{id}/students

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns the student roster for the given class. The class must exist first. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The class ID. |

Response body shape:

```json
[
  { "...": "student dto fields" }
]
```

Possible errors:

- `404 Not Found` if the class does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### POST /api/classes

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Creates a new class. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Class name. |
| gradeId | int | Yes | The grade the class belongs to. |
| schoolYear | short | Yes | School year. |
| homeroomUserId | int | Yes | The user ID of the homeroom teacher. |

Response body shape:

```json
{ "...": "created class dto fields" }
```

Possible errors:

- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### PUT /api/classes/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Updates an existing class. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Updated class name. |
| gradeId | int | Yes | Updated grade. |
| schoolYear | short | Yes | Updated school year. |
| homeroomUserId | int | Yes | Updated homeroom teacher user ID. |

Response body shape:

```json
{ "...": "updated class dto fields" }
```

Possible errors:

- `404 Not Found` if the class does not exist.
- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### DELETE /api/classes/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Deletes a class. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The class ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the class does not exist.
- `400 Bad Request` if the class cannot be deleted because of related data.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Subjects

### GET /api/subjects

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns a paginated list of subjects. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### GET /api/subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns one subject by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The subject ID. |

Response body shape:

```json
{ "...": "subject dto fields" }
```

Possible errors:

- `404 Not Found` if the subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### POST /api/subjects

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Creates a new subject. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Subject name. |

Response body shape:

```json
{ "...": "created subject dto fields" }
```

Possible errors:

- `400 Bad Request` for validation failures.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### PUT /api/subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Updates an existing subject. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Updated subject name. |

Response body shape:

```json
{ "...": "updated subject dto fields" }
```

Possible errors:

- `404 Not Found` if the subject does not exist.
- `400 Bad Request` for validation failures.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### DELETE /api/subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Deletes a subject. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The subject ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Class-Subjects

### GET /api/class-subjects

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns a paginated list of class-subject assignments. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### GET /api/class-subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Returns one class-subject assignment by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The class-subject ID. |

Response body shape:

```json
{ "...": "class-subject dto fields" }
```

Possible errors:

- `404 Not Found` if the class-subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### POST /api/class-subjects

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Creates a class-subject link and assigns the responsible teacher. The `teacherUserId` must belong to a user with the `teacher` role. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | The class being linked. |
| subjectId | int | Yes | The subject being linked. |
| teacherUserId | int | Yes | The teacher assigned to this class-subject. |

Response body shape:

```json
{ "...": "created class-subject dto fields" }
```

Possible errors:

- `400 Bad Request` for invalid IDs or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### PUT /api/class-subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Updates a class-subject assignment, usually to change the assigned teacher. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | Updated class ID. |
| subjectId | int | Yes | Updated subject ID. |
| teacherUserId | int | Yes | Updated teacher user ID. |

Response body shape:

```json
{ "...": "updated class-subject dto fields" }
```

Possible errors:

- `404 Not Found` if the class-subject does not exist.
- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

### DELETE /api/class-subjects/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Deletes a class-subject assignment. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The class-subject ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the class-subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Teachers

### GET /api/teachers/my-class-subjects

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Returns all class-subject assignments for the calling teacher. Each item includes the roster of students in that class. |

Response body shape:

```json
[
  {
    "id": 1,
    "classId": 2,
    "className": "Grade 7A",
    "subjectId": 3,
    "subjectName": "Mathematics",
    "teacherUserId": 15,
    "teacherName": "Jane Doe",
    "students": [
      {
        "id": 101,
        "name": "Student One",
        "sex": "Male"
      }
    ]
  }
]
```

Possible errors:

- `200 OK` with an empty list if the teacher has no assigned class-subjects.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

## Students

### GET /api/students

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns a paginated list of all students. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/students/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns one student by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The student ID. |

Response body shape:

```json
{ "...": "student dto fields" }
```

Possible errors:

- `404 Not Found` if the student does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### POST /api/students

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Creates a new student. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Student name. |
| sex | enum | Yes | `male` or `female`. |
| dob | ISO date | Yes | Date of birth. |
| classId | int | Yes | Class assignment. |

Response body shape:

```json
{ "...": "created student dto fields" }
```

Possible errors:

- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### PUT /api/students/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Updates an existing student. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| name | string | Yes | Updated student name. |
| sex | enum | Yes | `male` or `female`. |
| dob | ISO date | Yes | Updated date of birth. |
| classId | int | Yes | Updated class assignment. |

Response body shape:

```json
{ "...": "updated student dto fields" }
```

Possible errors:

- `404 Not Found` if the student does not exist.
- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### DELETE /api/students/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Deletes a student. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The student ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the student does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/students/class/{classId}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `teacher` |
| Description | Returns the students in a specific class. Teachers can read class rosters for their work. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | The class ID. |

Response body shape:

```json
[
  { "...": "student dto fields" }
]
```

Possible errors:

- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/students/parent/{parentUserId}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `parent` |
| Description | Returns the students linked to a parent. Parents can only request their own linked children; the server blocks any other parent ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| parentUserId | int | Yes | Parent user ID. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| page | int | No | Default is `1`. Must be greater than `0`. |
| pageSize | int | No | Default is `20`. Must be greater than `0`. |

Response body shape:

```json
{
  "items": ["..."],
  "page": 1,
  "pageSize": 20,
  "totalCount": 0
}
```

Possible errors:

- `400 Bad Request` if `page` or `pageSize` is less than `1`.
- `403 Forbidden` if a parent tries to read another parent’s children.
- `404 Not Found` if the parent has no linked children or the lookup fails.
- `401 Unauthorized` if the JWT is missing or invalid.

### POST /api/students/{id}/link-parent

| Item | Details |
| --- | --- |
| Roles | `super_admin` |
| Description | Links a parent user to a student. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | The student ID. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| parentUserId | int | Yes | Parent user to link. |
| studentId | int | Yes | Student to link. Usually this matches the route `id`. |

Response body shape:

```json
{ "message": "Parent linked successfully" }
```

Possible errors:

- `400 Bad Request` for validation failures or business rule violations.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin`.

## Attendance

### POST /api/attendance

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Creates one attendance record. The teacher can only mark attendance for class-subjects assigned to them. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student being marked. |
| classSubjectId | int | Yes | Assigned class-subject. Must belong to the calling teacher. |
| date | ISO date | Yes | Attendance date. |
| status | enum | Yes | `present`, `informed_absence`, or `uninformed_absence`. |

Response body shape:

```json
{ "...": "attendance dto fields" }
```

Possible errors:

- `400 Bad Request` if the class-subject does not belong to the calling teacher or if validation fails.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### POST /api/attendance/bulk

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Creates multiple attendance records in one request. Each entry must belong to a class-subject assigned to the calling teacher. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| entries | array | Yes | List of attendance entries to create. |

Each entry contains:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student being marked. |
| classSubjectId | int | Yes | Assigned class-subject. |
| date | ISO date | Yes | Attendance date. |
| status | enum | Yes | `present`, `informed_absence`, or `uninformed_absence`. |

Response body shape:

```json
{}
```

Possible errors:

- `400 Bad Request` if any entry violates the ownership rule or validation fails.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### GET /api/attendance/student/{studentId}?startDate=&endDate=

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `teacher` |
| Description | Returns attendance records for one student, optionally filtered by date range. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student ID. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| startDate | ISO date? | No | Start of the date range. |
| endDate | ISO date? | No | End of the date range. |

Response body shape:

```json
[
  { "...": "attendance dto fields" }
]
```

Possible errors:

- `404 Not Found` if the student does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/attendance/summary/{studentId}?month=&schoolYear=

| Item | Details |
| --- | --- |
| Roles | Any authenticated role |
| Description | Returns a summary count of informed and uninformed absences for a student in one month and school year. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student ID. |

Query parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| month | int | Yes | Month number from `1` to `12`. |
| schoolYear | short | Yes | School year. |

Response body shape:

```json
{
  "studentId": 1,
  "informedAbsences": 0,
  "uninformedAbsences": 0
}
```

Possible errors:

- `400 Bad Request` if `month` is not between `1` and `12`.
- `404 Not Found` if the student does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.

## Gradebook

### POST /api/gradebook

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Creates a gradebook entry. Gradebook entries are working notes for teachers and are not final scores. Teachers can only create entries for class-subjects assigned to them. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student being scored. |
| classSubjectId | int | Yes | Assigned class-subject. Must belong to the calling teacher. |
| label | string | Yes | Entry label such as quiz name. |
| score | decimal | Yes | The earned score. |
| maxScore | decimal | Yes | The maximum possible score. |

Response body shape:

```json
{ "...": "gradebook entry dto fields" }
```

Possible errors:

- `400 Bad Request` if the teacher is not assigned to the class-subject or if validation fails.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### GET /api/gradebook/{id}

| Item | Details |
| --- | --- |
| Roles | `teacher`, `homeroom` |
| Description | Returns one gradebook entry by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | Gradebook entry ID. |

Response body shape:

```json
{ "...": "gradebook entry dto fields" }
```

Possible errors:

- `404 Not Found` if the gradebook entry does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### PUT /api/gradebook/{id}

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Updates the label, score, or max score of an existing gradebook entry. It does not change student or class-subject. The same ownership rule as create applies. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| label | string | Yes | Updated label. |
| score | decimal | Yes | Updated score. |
| maxScore | decimal | Yes | Updated maximum score. |

Response body shape:

```json
{ "...": "updated gradebook entry dto fields" }
```

Possible errors:

- `404 Not Found` if the gradebook entry does not exist.
- `400 Bad Request` if the teacher is not assigned to the class-subject or if validation fails.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### DELETE /api/gradebook/{id}

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Deletes a gradebook entry. The same ownership rule as create applies. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | Gradebook entry ID. |

Response body shape:

```json
{}
```

Possible errors:

- `404 Not Found` if the gradebook entry does not exist.
- `400 Bad Request` if the teacher is not assigned to the class-subject.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### GET /api/gradebook/class-subject/{classSubjectId}

| Item | Details |
| --- | --- |
| Roles | `teacher`, `homeroom` |
| Description | Returns all gradebook entries for one class-subject, ordered by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classSubjectId | int | Yes | Class-subject ID. |

Response body shape:

```json
[
  { "...": "gradebook entry dto fields" }
]
```

Possible errors:

- `404 Not Found` if the class-subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

### GET /api/gradebook/student/{studentId}/class-subject/{classSubjectId}

| Item | Details |
| --- | --- |
| Roles | `teacher`, `homeroom` |
| Description | Returns all gradebook entries for one student in one class-subject, ordered by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student ID. |
| classSubjectId | int | Yes | Class-subject ID. |

Response body shape:

```json
[
  { "...": "gradebook entry dto fields" }
]
```

Possible errors:

- `404 Not Found` if the class-subject does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller lacks access.

## Monthly Scores

### POST /api/monthly-scores/submit

| Item | Details |
| --- | --- |
| Roles | `teacher` |
| Description | Submits a monthly final score for a student. If a score already exists for the same student, class-subject, month, and school year, the server updates it instead of creating a duplicate. The teacher can only submit for assigned class-subjects. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| studentId | int | Yes | Student ID. |
| classSubjectId | int | Yes | Assigned class-subject. Must belong to the calling teacher. |
| month | int | Yes | Month from `1` to `12`. |
| schoolYear | short | Yes | School year. |
| finalScore | decimal | Yes | Final monthly score. |

Response body shape:

```json
{ "...": "monthly score dto fields" }
```

Possible errors:

- `400 Bad Request` if the teacher is not assigned to the class-subject or if validation fails.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not a teacher.

### PUT /api/monthly-scores/{id}

| Item | Details |
| --- | --- |
| Roles | `homeroom` |
| Description | Updates an existing monthly score. This only works before the score is locked. The student, class-subject, month, and school year cannot be changed. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| finalScore | decimal | Yes | New score value. |

Response body shape:

```json
{ "...": "updated monthly score dto fields" }
```

Possible errors:

- `400 Bad Request` if the score is locked and cannot be edited.
- `404 Not Found` if the score does not exist.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `homeroom`.

## Monthly Reports

### POST /api/monthly-reports/submit

| Item | Details |
| --- | --- |
| Roles | `homeroom` |
| Description | Submits a monthly report for a class. The server checks that the report does not already exist, locks all monthly scores for that class/month/year, computes ranks, and saves the report and its entries in one transaction. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | Class being reported on. |
| month | int | Yes | Month from `1` to `12`. |
| schoolYear | short | Yes | School year. |

Response body shape:

```json
{ "...": "monthly report dto fields" }
```

Possible errors:

- `400 Bad Request` if the report already exists or if scores are incomplete.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `homeroom`.

### GET /api/monthly-reports/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `parent` |
| Description | Returns one monthly report. Parents can only view reports for classes that belong to their linked children. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | Monthly report ID. |

Response body shape:

```json
{
  "id": 1,
  "classId": 1,
  "month": 1,
  "schoolYear": 2025,
  "createdAt": "2026-04-26T00:00:00Z",
  "entries": [
    { "studentId": 1, "totalScore": 0, "rank": 1 }
  ]
}
```

Possible errors:

- `404 Not Found` if the report does not exist.
- `403 Forbidden` if a parent tries to view a report for a class that is not linked to their children.
- `401 Unauthorized` if the JWT is missing or invalid.

## Semester Reports

### POST /api/semester-reports/submit

| Item | Details |
| --- | --- |
| Roles | `homeroom` |
| Description | Submits a semester report for a class. Semester `1` covers months `1` to `6`; semester `2` covers months `7` to `12`. All monthly scores in the period must already be locked. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | Class being reported on. |
| semester | int | Yes | `1` or `2`. |
| schoolYear | short | Yes | School year. |

Response body shape:

```json
{ "...": "semester report dto fields" }
```

Possible errors:

- `400 Bad Request` if the report already exists or if monthly scores are incomplete or not locked.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `homeroom`.

### GET /api/semester-reports/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `parent` |
| Description | Returns one semester report. Parent access is limited to reports for classes linked to their children. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | Semester report ID. |

Response body shape:

```json
{
  "id": 1,
  "classId": 1,
  "semester": 1,
  "schoolYear": 2025,
  "createdAt": "2026-04-26T00:00:00Z",
  "entries": [
    { "studentId": 1, "totalScore": 0, "rank": 1 }
  ]
}
```

Possible errors:

- `404 Not Found` if the report does not exist.
- `403 Forbidden` if a parent tries to view a report for an unrelated class.
- `401 Unauthorized` if the JWT is missing or invalid.

## Yearly Reports

### POST /api/yearly-reports/submit

| Item | Details |
| --- | --- |
| Roles | `homeroom` |
| Description | Submits a yearly report for a class. The server requires both semester reports for the school year to exist first. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | Class being reported on. |
| schoolYear | short | Yes | School year. |

Response body shape:

```json
{ "...": "yearly report dto fields" }
```

Possible errors:

- `400 Bad Request` if semester scores are incomplete or if the report already exists.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `homeroom`.

### GET /api/yearly-reports/{id}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `parent` |
| Description | Returns one yearly report. Parent access is limited to reports for classes linked to their children. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| id | int | Yes | Yearly report ID. |

Response body shape:

```json
{
  "id": 1,
  "classId": 1,
  "schoolYear": 2025,
  "createdAt": "2026-04-26T00:00:00Z",
  "entries": [
    { "studentId": 1, "totalScore": 0, "rank": 1 }
  ]
}
```

Possible errors:

- `404 Not Found` if the report does not exist.
- `403 Forbidden` if a parent tries to view a report for an unrelated class.
- `401 Unauthorized` if the JWT is missing or invalid.

## Feedback

### POST /api/feedback

| Item | Details |
| --- | --- |
| Roles | `parent` |
| Description | Submits feedback for a monthly, semester, or yearly report. The server decides which foreign key column to fill based on `reportType`. Parents can only submit feedback for reports linked to their own children. |

Request body:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| reportType | enum | Yes | `monthly`, `semester`, or `yearly`. |
| reportId | int | Yes | The target report ID. |
| message | string | Yes | The feedback text. |

Response body shape:

```json
{ "...": "feedback dto fields" }
```

Possible errors:

- `400 Bad Request` if the report is not found or the parent is not authorized to submit feedback for it.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `parent`.

### GET /api/feedback/report/{reportType}/{reportId}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom`, `parent` |
| Description | Returns all feedback for one report, ordered by ID. Parents can only view feedback for reports tied to their children. Empty array is returned when no feedback exists. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| reportType | string | Yes | `monthly`, `semester`, or `yearly`. |
| reportId | int | Yes | Report ID. |

Response body shape:

```json
[
  { "...": "feedback dto fields" }
]
```

Possible errors:

- `400 Bad Request` if `reportType` is not one of `monthly`, `semester`, or `yearly`.
- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if a parent tries to view feedback for an unrelated report.

### GET /api/feedback/class/{classId}

| Item | Details |
| --- | --- |
| Roles | `super_admin`, `homeroom` |
| Description | Returns all feedback for a class across monthly, semester, and yearly reports, ordered by ID. |

Route parameters:

| Field | Type | Required | Notes |
| --- | --- | --- | --- |
| classId | int | Yes | Class ID. |

Response body shape:

```json
[
  { "...": "feedback dto fields" }
]
```

Possible errors:

- `401 Unauthorized` if the JWT is missing or invalid.
- `403 Forbidden` if the caller is not `super_admin` or `homeroom`.

## Common Mistakes

- Forgetting the `Bearer` prefix in the `Authorization` header.
- Calling an endpoint with the wrong role, especially teacher-only or super_admin-only routes.
- Trying to edit a monthly score after it has been locked by report submission.
- Trying to read or submit feedback for another parent’s child.
- Submitting a monthly, semester, or yearly report when one already exists for the same period.
