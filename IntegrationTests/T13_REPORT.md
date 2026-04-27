# T13 Report

## Files Created
- SchoolSystem.Desktop/Services/ApiClient.cs
- IntegrationTests/T13_REPORT.md

## Files Modified
- SchoolSystem.Desktop/Forms/Auth/frmLogin.cs
- SchoolSystem.Desktop/Program.cs

## Key Decisions
- Implemented ApiClient as a sealed lazy singleton with one shared static HttpClient and in-memory JWT storage only.
- Added JWT payload parsing using base64url decode + System.Text.Json to read name and role claims (role supports string or array).
- Applied Authorization header on each request based on current token and cleared it on logout.
- Implemented API error handling to prefer response message field; otherwise fallback to HTTP status code.
- Wired frmLogin code-behind only (no designer edits): async login click flow, Enter key submit on password, and automatic error clearing on input changes.
- Updated startup flow to show login dialog first, exit when canceled/failed, then run existing non-login shell form resolved from current assembly (preferred known shell names first, then first available form fallback).
