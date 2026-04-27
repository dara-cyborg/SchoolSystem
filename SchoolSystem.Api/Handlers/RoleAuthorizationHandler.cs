using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SchoolSystem.Api.Handlers;

public class SuperAdminHandler : AuthorizationHandler<SuperAdminRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAdminRequirement requirement) {
        var hasRole = context.User.FindAll(ClaimTypes.Role)
            .Any(c => c.Value == "SuperAdmin");

        if (hasRole) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class SuperAdminRequirement : IAuthorizationRequirement {
}

public class TeacherRequirement : IAuthorizationRequirement {
}

public class TeacherHandler : AuthorizationHandler<TeacherRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeacherRequirement requirement) {
        var hasRole = context.User.FindAll(ClaimTypes.Role)
            .Any(c => c.Value == "Teacher" || c.Value == "SuperAdmin");

        if (hasRole) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class HomeroomRequirement : IAuthorizationRequirement {
}

public class HomeroomHandler : AuthorizationHandler<HomeroomRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HomeroomRequirement requirement) {
        var hasRole = context.User.FindAll(ClaimTypes.Role)
            .Any(c => c.Value == "Homeroom" || c.Value == "SuperAdmin");

        if (hasRole) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class ParentHandler : AuthorizationHandler<ParentRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ParentRequirement requirement) {
        var hasRole = context.User.FindAll(ClaimTypes.Role)
            .Any(c => c.Value == "Parent" || c.Value == "SuperAdmin");

        if (hasRole) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class ParentRequirement : IAuthorizationRequirement {
}