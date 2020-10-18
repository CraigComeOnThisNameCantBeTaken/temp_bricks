import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ICustomer } from '../../Models/Customer';

// Not following MVVM but time constraints

@Component({
    selector: 'app-customer',
    templateUrl: './customer.component.html'
})
export class CustomerComponent implements OnInit {
    constructor(private http: HttpClient) { }

    customer: ICustomer[];

    // ordinarily I would use a typed form group
    // but adding to the project didnt work for some reason
    form = new FormGroup({
        id: new FormControl(null, Validators.required),
        firstName: new FormControl(null, Validators.required),
        surname: new FormControl(null, Validators.required),
        phoneNumber: new FormControl(null),
        email: new FormControl(null, [Validators.required, Validators.email]),
    });

    ngOnInit(): void {
        this.http.get<ICustomer[]>('api/customer')
            .subscribe(res => this.customer = res);
    }

    submit(): void {
        if (this.form.dirty && this.form.valid) {
            this.http.post('api/customer', this.form.getRawValue())
                .subscribe(
                    res => this.form.reset(),
                    (err: HttpErrorResponse) => alert(err.message)
                );
        }
    }
}
