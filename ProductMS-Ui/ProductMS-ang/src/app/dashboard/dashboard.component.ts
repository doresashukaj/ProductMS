import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']  
})
export class DashboardComponent {

  constructor(
    private router: Router,
    private authService: AuthService 
  ) {}

  onLogout() {
    this.authService.deleteToken();  
    this.router.navigateByUrl('/login');  
  }
}
