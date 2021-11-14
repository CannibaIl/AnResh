<template>
  <section>
    <b-modal
      id="modal-department"
      v-model="show"
      :title="title"
      @hidden="resetModal"
      hide-footer
    >
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

            <b-form-invalid-feedback id="employee-input-lastName-live-feedback">
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

            <b-form-invalid-feedback id="employee-input-salary-live-feedback">
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
          <b-button @click="show = false">Close</b-button>
          <b-button
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
    </b-modal>
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, maxLength } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],

  data() {
    return {
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
  },

  async mounted() {
    let { data } = await this.$axios.get("/api/department");
    this.departments = data;
  },

  methods: {
    open(employee) {
      this.employee = employee;
      this.title = !!this.employee ? "Edit" : "Create";
      this.show = true;

      this.form.firstName = employee?.firstName;
      this.form.lastName = employee?.lastName;
      this.form.middleName = employee?.middleName;
      this.form.departmentId = employee?.departmentId;
      this.form.salary = employee?.salary;
    },
    formatter(value) {
      if (value[0] !== undefined) {
        return value[0].toUpperCase() + value.slice(1).toLowerCase();
      }
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
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
        this.$axios
          .post("/api/employee/", this.form)
          .then((d) => {
            const departmentName = this.departments.find(
              (el) => el.id === this.form.departmentId
            ).name;
            d.data.response.departmentName = departmentName;
            this.$emit("row-created", d.data.response);
            this.show = false;
            this.$notifyInfo(
              "ADDED EMPLOYEE",
              `${this.$getFullName(d.data.response)}`
            );
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
            this.show = false;
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