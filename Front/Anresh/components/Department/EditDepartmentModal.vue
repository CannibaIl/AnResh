<template>
  <section>
    <b-modal 
      v-model="show" 
      :title="title" 
      @hidden="resetModal" 
      hide-footer
    >
      <b-form @submit.prevent="onSubmit">
        <b-form-group label="Name*" label-for="department-input-name" > 
          <b-form-input
            v-model="$v.form.name.$model"
            :state="validateState('name')"
            aria-describedby="department-input-name-live-feedback"
          ></b-form-input>

          <b-form-invalid-feedback id="department-input-name-live-feedback">
            {{this.$v.form.name.required ? 'Please enter no more than 20 characters.' : 'This field is required.'}}
          </b-form-invalid-feedback>
        </b-form-group>

        <footer class="modal-footer p-0 pt-3">
            <b-button @click="show = false">Close</b-button>
            <b-button v-bind:disabled="$v.form.$invalid" type="submit" variant="primary">Save</b-button>
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
      departament: null,
      title: '',
      form: {
        name: null
      }
    };
  },

  validations: {
    form: {
      name: {
        required,
        maxLength: maxLength(20)
      }
    }
  },

  methods: {
    open(department) {
      this.department = department
      this.title = !!this.department ? `Edit ${this.department.name}` : 'Create department'
      this.form.name = department?.name
      this.show = true
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },

    resetModal() {
      this.form = {
        name: null
      }
      this.$nextTick(() => {
        this.$v.$reset();
      });
    },

    async onSubmit() {
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        return;
      }
      if(!this.department) {
        await this.$axios.post("/api/department/", this.form)
        .then(d => {
          d.data.response.employeeCount = 0;
          this.$emit('row-created', d.data.response);
          this.show = false;
          this.$notifyInfo('CREATED DEPARTMENT',`${d.data.response.name}`);
        })
        .catch(error => {
          this.$notifyError('ERROR',`${error}`);
        });
      }
      else {
        this.form.id = this.department.id;
        await this.$axios.put("/api/department/", this.form)
        .then(d => {
          this.$notifyInfo('UPDATED DEPARTMENT',`${d.data.name}`);
          this.$emit('row-updated', {
            oldDepartment: this.department,
            newDepartment: d.data
          });
          this.show = false;
        })
        .catch(error => {
          this.$notifyError('ERROR',`${error}`);
        });
      }
    }

  }
};
</script>