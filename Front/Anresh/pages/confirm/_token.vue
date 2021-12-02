<template>
<section class="empty-section">
  <div class="form-wrap">
    <h1>Register</h1>
    <b-form @submit.prevent="onSubmit" class="auth-form">

      <b-form-group label="New password*" class="password-group">
        <b-form-input
          class="password-input"
          :type="passwordType"
          v-model="$v.password.$model"
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
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, minLength } from "vuelidate/lib/validators";

export default {
  layout: 'empty',
  mixins: [validationMixin],
  data() {
    return {
      passwordType: 'password',
      password: '',
    };
  },
  validations: {
    password: {
        required,
        minLength: minLength(6),
      },
  },
  methods: {
    validateState(name) {
      // const { $dirty, $error } = this.$v.[name];
      // return $dirty ? !$error : null;
    },
    async onSubmit() {
      await this.$axios
        .post(`api/user/confirm?Token=${this.$route.params.token}&Password=${this.password}`)
        .then((d) => {
          this.$router.replace('/login')
        })
        .catch((err) => {
            this.errors.push(err.response.data.errors)
        });
    },
  },
};
</script>