<template>
  <section>
    <b-modal
      :title="title"
      v-model="show"
      @hidden="resetModal"
      hide-footer
    >
      <b-form  @submit.prevent="onSubmit">
        <div v-if="!!employees">
          <div class="table-wrap mb-3">
            <table class="table table-sm mb-0 table-borderless">
              <thead>
                <tr>
                  <th scope="col-auto">Employees</th>
                </tr>
              </thead>
              <tbody >
                <tr class="table-string" v-for="employee of employees" :key="employee.id">
                  <td>{{ $getFullName(employee) }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <b-form-group>
            <b-form-radio v-model="$v.form.isDeleteEmployees.$model" :value=true>Delete employees</b-form-radio>
            <b-form-radio v-show="availableDepartments.length" v-model="$v.form.isDeleteEmployees.$model" :value=false>Transfer employees</b-form-radio>
          </b-form-group>

          <b-form-group v-show="form.isDeleteEmployees === false && availableDepartments.length" label="Department">
            <b-form-select
              v-model="$v.form.selectedDepartmentId.$model"
              :options="availableDepartments"
              class="mb-3"
              value-field="id"
              text-field="name"
              disabled-field="disabled"
            >
              <b-form-select-option :value="null" disabled>-- Please select department --</b-form-select-option>
            </b-form-select>

          </b-form-group>
        </div>
        <span v-else>There is no employees in the department</span>

        <footer class="modal-footer p-0 pt-3 mt-3">
            <b-button @click="show = false" class="custom-btn">Close</b-button>
            
            <b-button v-bind:disabled="$v.form.$invalid" 
            type="submit" 
            variant="danger"
            class="custom-btn"
            >Delete</b-button>
        </footer>
    
      </b-form>
    </b-modal>
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, requiredIf } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  props: ['departments'],

  data() { 
    return {
      department: null,
      title: '',
      show: false,
      availableDepartments: [], 
      employees: null,
      form: {
        isDeleteEmployees: null,
        selectedDepartmentId: null,
      },
    };
  },

  validations: {
    form: {
      isDeleteEmployees: {
        required: requiredIf(function () {
            return this.employees !== null;
        })
      },
      selectedDepartmentId: {
        required: requiredIf(function () {
            return this.form.isDeleteEmployees === false;
        })
      }
    }
  },

  methods: {
    async open(department) {
        this.department = department
        this.title = `Delete ${department.name}`
        this.form.isDeleteEmployees = null;
        this.availableDepartments = this.departments.filter(d => d !== department);
        
        if(department.employeeCount > 0) {
          const {data} = await this.$axios.get(`/api/employee/all/department/${department.id}`);
          this.employees = data;
        }
        
        this.show = true;
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },

    resetModal() {
      this.form = {
        selectedDepartmentId: null,
        isDeleteEmployees: null
      }
      this.employees = null;
      this.$nextTick(() => {
        this.$v.$reset();
      });
    },

    deleteDepartment(id) {
      this.$axios.delete("/api/department/" + id)
          .then(() => {
            this.show = false;
            this.$emit('row-updated');
            this.$notifyWarn('DELETED DEPARTMENT',`${this.department.name}`);
            })
          .catch(error => {
              this.$notifyError('ERROR',`${error}`);
          });
    },

    onSubmit() {
      if (this.$v.form.$anyError && this.employees === null) {
         return;
      }
      const id = this.department.id;
      if(this.employees === null) {
        this.deleteDepartment(id);
      } 
      else {
        if(this.form.isDeleteEmployees) {
          this.$axios.delete('/api/employee/department/' + id)
            .then(() => {
              this.$notifyWarn('DELETED EMPLOYEES FROM THE DEPARTMENT:',`${this.department.name}`);
              this.deleteDepartment(id);
            })
            .catch(error => {
                this.$notifyError('ERROR',`${error}`);
            });
        } 
        
        else {
          let payload = { oldDepartmentId: id, newDepartmentId: this.form.selectedDepartmentId };
          this.$axios.put("/api/department/move-employees", payload)
            .then(({ data }) => {
              this.$notifyInfo('EMPLOYEES HAS BEEN TRANSFERRED!','');
              this.deleteDepartment(id);
              this.$emit('row-updated');
            })
            .catch(error => {
                this.$notifyError('ERROR',`${error}`);
            });
        }
      }
    }

  }
};
</script>