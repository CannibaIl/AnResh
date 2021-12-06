<template>
  <section>
    <b-modal v-model="show" :title="title" @hidden="resetModal" hide-footer>
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
                    step="0.01"
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

                <!-- <b-form-group label="Department*">
                  <b-form-select
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
                </b-form-group> -->
                <!-- *************************************************************************************************************** -->

                <div class="department-parents-wrap">
                  <label @click="setdepartments(null, null)">/ root</label>
                  <label
                    class="d-inline mr-1"
                    v-for="(departmentParent, index) in departmentParents"
                    :key="departmentParent.name"
                    @click="setdepartments(departmentParent, index)"
                    >/ {{ departmentParent.name }}
                  </label>
                  <label> *</label>

                  <ul class="select-department">
                    <li
                      v-for="department in departments"
                      :key="department.id"
                      @click="setdepartments(department, null)"
                      :class="
                        department.id === form.departmentId
                          ? 'active-department'
                          : '' " >
                      {{ department.name }}
                    </li>
                  </ul>
                </div>
                <div
                  class="invalid-feedback d-block"
                  v-if="form.departmentId === null"
                >
                  his field is required.
                </div>

                <!-- *************************************************************************************************************** -->
                <b-form-group label="Skills:">
                  <label
                    class="d-inline mr-1"
                    v-for="skill in selectedSkills"
                    :key="skill.name + skill.id"
                    >{{ skill.name }},
                  </label>
                  <div class="select-skill">
                    <span v-for="skill in skills" :key="skill.id + skill.name">
                      <b-form-checkbox v-model="form.skills" :value="skill">{{
                        skill.name
                      }}</b-form-checkbox>
                    </span>
                  </div>
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
                  v-bind:disabled="$v.form.$invalid || disabledSaveButton"
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
                  :disabled="this.handleAccountButton !== 'Create'"
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

              <footer class="modal-footer p-0 pt-3">
                <b-button class="custom-btn" @click="show = false"
                  >Close</b-button
                >
                <b-button class="custom-btn" type="submit" variant="primary">{{
                  handleAccountButton
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
  minValue,
  email,
} from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],

  data() {
    return {
      disabledAccountTab: true,
      disabledSaveButton: false,
      handleAccountButton: "Create",
      show: false,
      employee: null,
      title: "",
      departments: [],
      departmentParents: [],
      skills: [],
      form: {
        firstName: null,
        lastName: null,
        middleName: null,
        departmentId: null,
        salary: null,
        skills: [],
      },
      account: {
        id: null,
        employeeId: null,
        email: null,
        role: null,
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
        minValue: minValue(1),
      },
      skills: {},
    },
    account: {
      email: {
        required,
        email,
      },
      role: {
        required,
      },
    },
  },
  computed: {
    selectedSkills() {
      return this.form.skills;
    },
  },
  methods: {
    async open(employee) {
      this.disabledAccountTab = !!employee ? false : true;
      this.title = !!employee ? "Edit" : "Create";

      let skillUri = "/api/skill";
      await this.$axios.get(skillUri).then((d) => {
        this.skills = d.data;
      });

      if (!!employee) {
        this.employee = employee;
        this.form.firstName = employee?.firstName;
        this.form.lastName = employee?.lastName;
        this.form.middleName = employee?.middleName;
        this.form.departmentId = employee?.departmentId;
        this.form.salary = employee?.salary;
        this.form.skills = !!employee.skills ? employee.skills : [];

        let { data } = await this.$axios.get(
          `/api/user/employee/${employee.id}`
        );

        this.account.id = data?.id;
        this.account.employeeId = data?.id;
        this.account.email = data?.email;
        this.account.role = data?.role;
        this.handleAccountButton = !!data.email ? "Change role" : "Create";
      }
      const departmentId =
        this.form.departmentId === null ? 0 : this.form.departmentId;
      let { data } = await this.$axios.get(
        `/api/department/childrenAndParents/childId/${departmentId}`
      );
      this.departments = data.children;
      this.departmentParents = data.parents;

      this.show = true;
    },
    formatter(value) {
      if (value[0] !== undefined) {
        return value[0].toUpperCase() + value.slice(1).toLowerCase();
      }
    },

    async setdepartments(department, index) {
      const departmentId = department === null ? 0 : department.id;
      let { data } = await this.$axios.get(
        `/api/department/parentId/${departmentId}`
      );
      if (data.length > 0) {
        this.departments = data;
      }

      if (department === null && index === null) {
        this.form.departmentId = null;
        this.departmentParents = [];
        return;
      }

      const parentsLength = this.departmentParents.length;
      const lastParent = this.departmentParents[parentsLength - 1];
      if (index !== null) {
        this.departmentParents.splice(index + 1, parentsLength - 1 - index);
      } else {
        if (
          lastParent !== undefined &&
          lastParent.parentId === department.parentId
        ) {
          this.departmentParents.splice(-1);
        }
        this.departmentParents.push(department);
      }
      this.form.departmentId = departmentId;
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name] || this.$v.account[name];
      return $dirty ? !$error : null;
    },

    resetModal() {
      this.disabledSaveButton = false;
      this.form = {
        firstName: null,
        lastName: null,
        middleName: null,
        departmentId: null,
        salary: null,
        skills: [],
      };
      this.account = {
        employeeId: null,
        email: null,
        role: null,
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
            const departmentName = this.departmentParents.find(
              (el) => el.id === this.form.departmentId
            ).name;
            d.data.response.departmentName = departmentName;
            this.$emit("row-updated");
            this.$notifyInfo(
              "ADDED EMPLOYEE",
              `${this.$getFullName(d.data.response)}`
            );
            this.account.employeeId = d.data.response.id;
            this.disabledAccountTab = false;
            this.disabledSaveButton = true;
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
            this.$emit("row-updated");
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      }
    },
    async handleUser() {
      if (this.handleAccountButton === "Create") {
        var request = {
          employeeId: this.employee.id,
          email: this.account.email,
          role: this.account.role,
        };
        await this.$axios
          .post("/api/user/create", request)
          .then((d) => {
            this.handleAccountButton = "Change role";
            this.$notifyInfo("ACCOUN CREATED", `${this.account.email}`);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      } else {
        var request = { id: this.account.id, role: this.account.role };
        await this.$axios
          .put("/api/user/role", request)
          .then((d) => {
            this.$notifyInfo("ROLE CHANGED", "");
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
.select-skill {
  overflow-y: scroll;
  max-height: 170px;
  border: 1px solid #ced4da;
  border-radius: 0.25rem;
  padding: 1rem;
}
</style>