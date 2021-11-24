<template>
  <section class="empty-section">
    <div class="form-wrap">
    <h1>Restore password</h1>
    <b-form @submit.prevent="onSubmit" class="auth-form">
      <b-form-group label="Enter new password*" class="password-group">
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

      <footer class="d-flex justify-content-center p-0 pt-3">
        <b-button class="custom-btn" type="submit" variant="primary"
          >Restore</b-button
        >
      </footer>
    </b-form>
    </div>
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, minLength } from "vuelidate/lib/validators";

export default {
  layout: "empty",
  mixins: [validationMixin],
  data() {
    return {
      passwordType: "password",
      form: {
        password: "",
      },
    };
  },
  validations: {
    form: {
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
      var request = {
        token: this.$route.params.token,
        password: this.form.password,
      };
      await this.$axios
        .post("/api/user/RestorePassword", request)
        .then((d) => {
          var request = {
            email: d.data.email,
            password: this.form.password,
          };
          this.$auth.loginWith("local", { data: request });
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

