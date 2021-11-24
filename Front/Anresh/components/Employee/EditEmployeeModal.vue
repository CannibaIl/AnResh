<template>
  <section>
    <b-modal
      id="modal-department"
      v-model="show"
      :title="title"
      @hidden="resetModal"
      hide-footer
    >
      <b-card no-body>
        <b-tabs card>
          <b-tab :title="title + ' data'" active>
            <b-form @submit.prevent="onSubmit">
              <div v-if="departments.length">
                <b-form-group label="FirstName*">
                  <b-form-input
                    v-model="$v.form.firstName.$model"
                    :formatter="formatter"
                    :state="validateState('firstName')"
                    aria-describedby="employee-input-firstName-live-feedback"
                  ></b-form-input>

                  <b-form-invalid-feedback
                    id="employee-input-firstName-live-feedback"
                  >
                    {{
                      this.$v.form.firstName.required
                        ? "Please enter no more than 20 characters."
                        : "This field is required."
                    }}
                  </b-form-invalid-feedback>
                </b-form-group>

                <b-form-group label="LastName*">
                  <b-form-input
                    v-model="$v.form.lastName.$model"
                    :formatter="formatter"
                    :state="validateState('lastName')"
                    aria-describedby="employee-input-lastName-live-feedback"
                  ></b-form-input>

                  <b-form-invalid-feedback
                    id="employee-input-lastName-live-feedback"
                  >
                    {{
                      this.$v.form.lastName.required
                        ? "Please enter no more than 20 characters."
                        : "This field is required."
                    }}
                  </b-form-invalid-feedback>
                </b-form-group>

                <b-form-group label="MiddleName">
                  <b-form-input
                    v-model="$v.form.middleName.$model"
                    :formatter="formatter"
                    :state="validateState('middleName')"
                    aria-describedby="employee-input-middleName-live-feedback"
                  ></b-form-input>

                  <b-form-invalid-feedback
                    id="employee-input-middleName-live-feedback"
                  >
                    {{
                      this.$v.form.middleName.required
                        ? "Please enter no more than 20 characters."
                        : "This field is required."
                    }}
                  </b-form-invalid-feedback>
                </b-form-group>

                <b-form-group label="Salary*">
                  <b-form-input
                    type="number"
                    v-model="$v.form.salary.$model"
                    :state="validateState('salary')"
                    aria-describedby="employee-input-salary-live-feedback"
                  ></b-form-input>

                  <b-form-invalid-feedback
                    id="employee-input-salary-live-feedback"
                  >
                    This field is required.
                  </b-form-invalid-feedback>
                </b-form-group>

                <b-form-group label="Department*">
                  <b-form-select
                    class="mb-3"
                    v-model="$v.form.departmentId.$model"
                    :options="departments"
                    value-field="id"
                    text-field="name"
                    disabled-field="disabled"
                  >
                    <b-form-select-option :value="form.departmentId" disabled
                      >-- Please select department --</b-form-select-option
                    >
                  </b-form-select>
                </b-form-group>
              </div>

              <div class="departments-is-empty" v-else>
                <img
                  class="smile"
                  src="https://cdn.rawgit.com/ahmedhosna95/upload/1731955f/sad404.svg"
                  alt="404"
                />
                <h3>The list of departments is empty!</h3>
                <p>First add a department</p>
              </div>

              <footer class="modal-footer p-0 pt-3">
                <b-button class="custom-btn" @click="show = false"
                  >Close</b-button
                >
                <b-button
                  class="custom-btn"
                  v-if="departments.length"
                  v-bind:disabled="$v.form.$invalid"
                  type="submit"
                  variant="primary"
                  >Save</b-button
                >
                <nuxt-link
                  v-if="!departments.length"
                  class="btn btn-primary"
                  style="line-height: 36px"
                  to="/departments"
                  >Go to departaments</nuxt-link
                >
              </footer>
            </b-form>
          </b-tab>
          <b-tab title="Account" v-bind:disabled="disabledAccountTab">
            <b-form @submit.prevent="handleUser">
              <b-form-group label="Email*">
                <b-form-input
                  :disabled="this.handleAccount !== 'Create'"
                  v-model="$v.account.email.$model"
                  :state="validateState('email')"
                  aria-describedby="input-email-live-feedback"
                ></b-form-input>

                <b-form-invalid-feedback id="input-email-live-feedback">
                  {{
                    this.$v.account.email.required
                      ? "Check the spelling of the email."
                      : "Please enter email."
                  }}
                </b-form-invalid-feedback>
              </b-form-group>

              <b-form-group label="Role*">
                <b-form-input
                  v-model="$v.account.role.$model"
                  :state="validateState('role')"
                  aria-describedby="input-role-live-feedback"
                ></b-form-input>

                <b-form-invalid-feedback id="input-role-live-feedback">
                  Сhoose a role
                </b-form-invalid-feedback>
              </b-form-group>

              <b-form-group
                v-if="this.handleAccount === 'Create'"
                label="Password*"
                class="password-group"
              >
                <b-form-input
                  class="password-input"
                  :state="validateState('password')"
                  v-model="$v.account.password.$model"
                  aria-describedby="input-password-live-feedback"
                >
                </b-form-input>
                <b-form-invalid-feedback id="input-password-live-feedback">
                  The minimum password length is 6 characters
                </b-form-invalid-feedback>
              </b-form-group>

              <footer class="modal-footer p-0 pt-3">
                <b-button class="custom-btn" @click="show = false"
                  >Close</b-button
                >
                <b-button class="custom-btn" type="submit" variant="primary">{{
                  handleAccount
                }}</b-button>
              </footer>
            </b-form>
          </b-tab>
        </b-tabs>
      </b-card>
    </b-modal>
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import {
  required,
  maxLength,
  minLength,
  email,
} from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],

  data() {
    return {
      disabledAccountTab: true,
      handleAccount: "",
      show: false,
      employee: null,
      title: "",
      departments: [],
      form: {
        firstName: null,
        lastName: null,
        middleName: null,
        departmentId: null,
        salary: null,
      },
      account: {
        id: null,
        employeeId: null,
        email: null,
        role: null,
        password: null,
      },
    };
  },

  validations: {
    form: {
      firstName: {
        required,
        maxLength: maxLength(20),
      },
      lastName: {
        required,
        maxLength: maxLength(20),
      },
      middleName: {
        maxLength: maxLength(20),
      },
      departmentId: {
        required,
      },
      salary: {
        required,
      },
    },
    account: {
      email: {
        required,
        email,
      },
      role: {
        required,
      },
      password: {
        required,
        minLength: minLength(6),
      },
    },
  },

  async mounted() {
    let { data } = await this.$axios.get("/api/department/simple");
    this.departments = data;
  },

  methods: {
    async open(employee) {

      this.disabledAccountTab = !!employee ? false : true;
      this.title = !!employee ? "Edit" : "Create";
      this.show = true;

      if(!!employee) {
        this.employee = employee;
        this.form.firstName = employee?.firstName;
        this.form.lastName = employee?.lastName;
        this.form.middleName = employee?.middleName;
        this.form.departmentId = employee?.departmentId;
        this.form.salary = employee?.salary;

        let { data } = await this.$axios.get(`/api/user/employee/${employee.id}`);
        
        this.account.id = data?.id;
        this.account.employeeId = data?.id;
        this.account.email = data?.email;
        this.account.role = data?.role;
        this.handleAccount = !!data.email ? "Change role" : "Create";
      }
    },
    formatter(value) {
      if (value[0] !== undefined) {
        return value[0].toUpperCase() + value.slice(1).toLowerCase();
      }
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name] || this.$v.account[name];
      return $dirty ? !$error : null;
    },

    resetModal() {
      this.form = {
        firstName: null,
        lastName: null,
        middleName: null,
        departmentId: null,
        salary: null,
      };
      this.account = {
        employeeId: null,
        email: null,
        role: null,
        password: null,
      };
      this.$nextTick(() => {
        this.$v.$reset();
      });
    },

    async onSubmit() {
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        return;
      }
      if (!this.employee) {
        await this.$axios
          .post("/api/employee/", this.form)
          .then((d) => {
            const departmentName = this.departments.find(
              (el) => el.id === this.form.departmentId
            ).name;
            d.data.response.departmentName = departmentName;
            this.$emit("row-created", d.data.response);
            this.$notifyInfo(
              "ADDED EMPLOYEE",
              `${this.$getFullName(d.data.response)}`
            );
            this.account.employeeId = d.data.response.id;
            this.disabledAccountTab = false;
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      } else {
        this.form.id = this.employee.id;
        this.$axios
          .put("/api/employee/", this.form)
          .then((d) => {
            const departmentName = this.departments.find(
              (el) => el.id === this.form.departmentId
            ).name;
            d.data.departmentName = departmentName;
            this.$notifyInfo(
              "UPDATED EMPLOYEE",
              `${this.$getFullName(d.data)}`
            );
            this.$emit("row-updated", d.data);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      }
    },
    async handleUser() {
      if (this.handleAccount === 'Create') {
        var request = {
          employeeId: this.employee.id,
          email: this.account.email,
          role: this.account.role,
          password: this.account.password,
        };
        await this.$axios
          .post("/api/user/create", request)
          .then((d) => {
            this.handleAccount = 'Change role';
            this.$notifyInfo("ACCOUN CREATED", `${this.account.email}`);
            console.log(d);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      } else {
        var request = { id: this.account.id, role: this.account.role };
        await this.$axios
          .put("/api/user/role", request)
          .then((d) => {
            this.$notifyInfo("ROLE CHANGED", '');
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      }
    },
  },
};
</script>


<style lang="scss">
.departments-is-empty {
  text-align: center;
  img {
    width: 100%;
  }
}
</style>