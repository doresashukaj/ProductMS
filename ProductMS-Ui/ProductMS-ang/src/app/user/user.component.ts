import { Component } from '@angular/core';
import { RegistrationComponent } from "./registration/registration.component";
import { RouterOutlet } from '@angular/router';
import { RouterLink } from '@angular/router';
import { DashboardComponent } from "../dashboard/dashboard.component";


@Component({
  selector: 'app-user',
  standalone:true,
  imports: [RegistrationComponent, RouterOutlet, DashboardComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {

  
}
