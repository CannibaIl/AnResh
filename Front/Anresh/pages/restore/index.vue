<template>
<section class="empty-section">
  <div class="auth-form">
    
    <h1>Restore password</h1>

  <b-form @submit.prevent="onSubmit">

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

      <footer class="d-flex justify-content-center p-0 pt-3 mt-5">
        <b-button class="custom-btn" type="submit" variant="primary">Restore</b-button>
      </footer>
  </b-form>
    </div>
</section>
   
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, email } from "vuelidate/lib/validators";

export default {
  layout: 'empty',
  mixins: [validationMixin],
  data() {
    return {
      form: {
        email: "",
      },
    };
  },
  validations: {
    form: {
      email: {
        required,
        email,
      },
    },
  },
  methods: {
    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },
    async onSubmit() {
     await this.$axios.post(`/api/user/sendRestorePasswordEmail/${this.form.email}`)
    .then((d) => {
          console.log(d)
        })
        .catch((error) => {
          console.log(error)
        });
    },
  },
};
</script>

