import { AppUserClaim } from "./app.user.claim";

export class AppUserAuth {
  userName: string = "";
  bearerToken: string = "";
  isAuthenticated: boolean = false;
  claims: AppUserClaim[] = [];
  //canViewReports: boolean = false;
  //canConfirmExpenses: boolean = false;
  //canViewExpenses: boolean = false;
  //canViewTransactions: boolean = false;
  //canAccessRest: boolean = false;
}
