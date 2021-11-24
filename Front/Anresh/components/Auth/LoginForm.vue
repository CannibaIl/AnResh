<template>
  <div>
    <h1>login</h1>
    <b-form @submit.prevent="onSubmit" class="auth-form">
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

      <footer class="d-flex justify-content-center p-0 pt-3 mt-5">
        <b-button class="custom-btn" type="submit" variant="primary"
          >Sign in</b-button
        >
      </footer>
      <div class="d-flex">
        <nuxt-link
          active-class="active"
          class="nav-link d-flex justify-content-center mt-2"
          to="/register"
          >register</nuxt-link
        >
        <nuxt-link
          active-class="active"
          class="nav-link d-flex justify-content-center mt-2"
          to="/restore"
          >restore password</nuxt-link
        >
      </div>
    </b-form>
  </div>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, minLength, email } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  data() {
    return {
      passwordType: "password",
      form: {
        email: "",
        password: "",
      },
    };
  },
  validations: {
    form: {
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
  methods: {
    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },
    async onSubmit() {
      try {
        await this.$auth.loginWith("local", { data: this.form });
      } catch (err) {
        console.log(err);
      }
    },
  },
};
</script>



<style lang="scss" scoped>
</style>
