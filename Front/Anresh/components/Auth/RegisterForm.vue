<template>
  <div>
    <h1>Register</h1>
    <b-form @submit.prevent="onSubmit" class="auth-form">
     
      <b-form-group label="Id*">
        <b-form-input
          v-model="$v.form.employeeId.$model"
          :state="validateState('employeeId')"
          aria-describedby="input-employeeId-live-feedback"
        ></b-form-input>

        <b-form-invalid-feedback id="input-employeeId-live-feedback">
         Please enter employee id.
        </b-form-invalid-feedback>
      </b-form-group>

      <b-form-group label="Email*">
        <b-form-input
          v-model="$v.form.email.$model"
          :state="validateState('email')"
          aria-describedby="input-email-live-feedback"
        ></b-form-input>

        <b-form-invalid-feedback id="input-email-live-feedback">
          {{
            this.$v.form.email.required
              ? "Check the spelling of the email."
              : "Please enter email."
          }}
        </b-form-invalid-feedback>
      </b-form-group>

      <b-form-group label="Password*" class="password-group">
        <b-form-input
          class="password-input"
          :type="passwordType"
          v-model="$v.form.password.$model"
          :state="validateState('password')"
          aria-describedby="input-password-live-feedback"
        >
        </b-form-input>
        <b-form-invalid-feedback id="input-password-live-feedback">
          The minimum password length is 6 characters
        </b-form-invalid-feedback>
        <b-form-group class="show-password-checkbox-group">
          <b-form-checkbox
            class="show-password-checkbox"
            v-model="passwordType"
            value="password"
            unchecked-value="text"
          >
          </b-form-checkbox>
          <fa
            class="checkbox-icon"
            v-show="passwordType == 'password'"
            icon="eye"
          />
          <fa
            class="checkbox-icon"
            v-show="passwordType == 'text'"
            icon="eye-slash"
          />
        </b-form-group>

        
      </b-form-group>

     <div v-if="errors.length !== 0">
        <li v-for="error in errors" :key="error">
          {{ error[0] }}
        </li>
      </div>

      <footer class="d-flex justify-content-center p-0 pt-3 mt-5">
        <b-button
        class="custom-btn"
          type="submit"
          variant="primary"
          >Register</b-button
        >
      </footer>

      <nuxt-link active-class="active" class="nav-link d-flex justify-content-center mt-2" to="/login">Sign in</nuxt-link>
    </b-form> 
  </div>
</template>

<script>
import { validationMixin } from "vuelidate";
import {
  required,
  minLength,
  maxLength,
  email,
  minValue,
} from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  data() {
    return {
      errors: [],
      departments: [],
      passwordType: 'password',
      form: {
        employeeId: '',
        email: '',
        password: '',
      },
    };
  },
  validations: {
    form: {
      employeeId: {
        required
      },
      email: {
        required,
        email,
      },
      password: {
        required,
        minLength: minLength(6),
      },
    },
  },
  async mounted() {
    let { data } = this.$axios.get("/api/department/simple");
    this.departments = data;
  },
  methods: {
    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },
    async onSubmit() {
      await this.$axios
        .post("/api/user/register", this.form)
        .then((d) => {
          this.$notifyInfo( "CONFIRM EMAIL",  `by email ${this.form.email}, sent confirmation link`);
        })
        .catch((err) => {
            this.errors.push(err.response.data.errors)
        });
    },
  },
};
</script>