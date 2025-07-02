import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SupportService } from '../../services/support.service';

@Component({
  selector: 'app-update-issue',
  templateUrl: './update-issue.component.html',
  styleUrls: ['./update-issue.component.css']
})
export class UpdateIssueComponent implements OnInit{
  issueId: number = 0;
  orderItemId: number = 0;
  email: string = '';
  issueDescription: string = '';
  Status: string = '';

  constructor(private _supportService: SupportService, private _route: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    this.issueId = this._route.snapshot.params['iId'];
    this.orderItemId = this._route.snapshot.params['oId'];

    this.issueDescription = this._route.snapshot.params['iDesc'];
    this.email = this._route.snapshot.params['email'];
    this.Status = this._route.snapshot.params['iStatus'];


  }

  updateIssueStatus(status: string,email:string) {
    this._supportService.UpdateIssueStatus(this.issueId, status).subscribe(
      (resSuccess) => {
        if (resSuccess) {
          alert("Issue Status Updated!!")
          if (status == "Resolved") {
            this._supportService.TestMail(email, status).subscribe(
              (mailRes) => {
                alert("Email sent to the Customer");
                this._router.navigate(['/view-issues']);
              },
              (mailErr) => {
                console.error(mailErr);
                alert("Issue updated but mail failed to send.");
                this._router.navigate(['/support']);
              }
            );
          }

        }
        else {
          alert("Issue status not updated..")
        }
        this._router.navigate(['/support'])
      },
      (resError) => {
        console.log(resError);
        alert("Error Occurred while updating the status")
      },
      () => { console.log("Issue status Updation executed successfully!!!"); }
    )
  }

}
