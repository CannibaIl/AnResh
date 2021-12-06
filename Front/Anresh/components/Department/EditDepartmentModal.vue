<template>
  <section>
    <b-modal v-model="show" :title="title" @hidden="resetModal" hide-footer>
      <b-form @submit.prevent="onSubmit">
        
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
                department.id === form.parentId ? 'active-department' : ''
              "
            >
              {{ department.name }}
            </li>
          </ul>
        </div>

        <b-form-group label="Name*" label-for="department-input-name">
          <b-form-input
            v-model="$v.form.name.$model"
            :state="validateState('name')"
            aria-describedby="department-input-name-live-feedback"
          ></b-form-input>

          <b-form-invalid-feedback id="department-input-name-live-feedback">
            {{
              this.$v.form.name.required
                ? "Please enter no more than 20 characters."
                : "This field is required."
            }}
          </b-form-invalid-feedback>
        </b-form-group>

        <footer class="modal-footer p-0 pt-3">
          <b-button class="custom-btn" @click="show = false">Close</b-button>
          <b-button
            class="custom-btn"
            v-bind:disabled="$v.form.$invalid"
            type="submit"
            variant="primary"
            >Save</b-button
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
      departament: null,
      title: "",
      departments: [],
      departmentParents: [],
      form: {
        parentId: null,
        name: null,
      },
    };
  },

  validations: {
    form: {
      name: {
        required,
        maxLength: maxLength(20),
      },
    },
  },

  methods: {
    async open(department) {
      this.department = department;
      this.title = !!this.department
        ? `Edit ${this.department.name}`
        : "Create department";
      this.form.parentId = department?.parentId;
      this.form.name = department?.name;

      let { data } = await this.$axios.get(`/api/department/childrenAndParents/childId/${department?.parentId || 0}`);
      this.departments = data.children;
      this.departmentParents = data.parents;

      this.show = true;
    },

    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },

    resetModal() {
      this.form = {
        parentId: null,
        name: null,
      };
      this.$nextTick(() => {
        this.$v.$reset();
      });
    },

    async setdepartments(department, index) {
      const departmentId = department === null ? 0 : department.id;
      let { data } = await this.$axios.get(`/api/department/parentId/${departmentId}`);
      if(data.length > 0) {
        this.departments = data;
      }

      if( department === null && index === null ) {
        this.form.parentId = null;
        this.departmentParents = [];
        return;
      }

      const parentsLength = this.departmentParents.length;
      const lastParent = this.departmentParents[parentsLength - 1];
      if(index !== null)
      {
        this.departmentParents.splice(index + 1 , parentsLength - 1 - index )
      }
      else {
        if(lastParent !== undefined && lastParent.parentId === department.parentId) {
          this.departmentParents.splice(-1); 
        }
        this.departmentParents.push(department);
      }
      this.form.parentId = departmentId;
    },

    async onSubmit() {
      this.$v.form.$touch();
      if (this.$v.form.$anyError) {
        return;
      }
      if (!this.department) {
        await this.$axios
          .post("/api/department/", this.form)
          .then((d) => {
            d.data.response.employeeCount = 0;
            this.$emit("row-updated");
            this.show = false;
            this.$notifyInfo("CREATED DEPARTMENT", `${d.data.response.name}`);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      } else {
        this.form.id = this.department.id;
        await this.$axios
          .put("/api/department/", this.form)
          .then((d) => {
            this.$notifyInfo("UPDATED DEPARTMENT", `${d.data.name}`);
            this.$emit("row-updated");
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