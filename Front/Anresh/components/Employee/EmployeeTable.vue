<template>
  <section>
    <edit-employee-modal
      ref="editEmployeeModal"
      @row-updated="updateDataTable"
    />
    <delete-employee-modal
      ref="deleteEmployeeModal"
      @row-updated="updateDataTable"
    />

    <b-button v-b-toggle.collapse-1 class="mb-3 custom-btn"
      >Filter
      <fa icon="filter" style="transform: scale(0.8)" />
    </b-button>
    <b-collapse id="collapse-1" class="mt-2 filter-wrap">
      <div class="left-side-filter">
        <div class="filter-inputs">
          <b-form-group label="First name">
            <b-form-input
              type="text"
              debounce="1000"
              v-model="pageFilter.firstName"
            ></b-form-input>
          </b-form-group>

          <b-form-group label="Min salary">
            <b-form-input
              type="number"
              step="1"
              min="0"
              debounce="1000"
              v-model="pageFilter.minSalary"
            ></b-form-input>
          </b-form-group>

          <b-form-group label="Max salary">
            <b-form-input
              type="number"
              step="1"
              min="0"
              debounce="1000"
              v-model="pageFilter.maxSalary"
            ></b-form-input>
          </b-form-group>
        </div>

        <b-form-group class="department-filter department-parents-wrap">
          <label @click="setDepartmentFilter(null, null)">/ departaments</label>
          <label
            class="d-inline mr-1"
            v-for="(departmentParent, index) in departmentParents"
            :key="departmentParent.name"
            @click="setDepartmentFilter(departmentParent, index)"
            >/ {{ departmentParent.name }}
          </label>

          <ul class="select-department">
            <li
              v-for="department in departments"
              :key="department.id"
              @click="setDepartmentFilter(department, null)"
            >
              {{ department.name }}
            </li>
          </ul>
        </b-form-group>
      </div>
      <b-form-group label="Skills:" class="ml-3">
        <div class="select-skill-wrap">
          <div
            class="select-skill-inner"
            v-for="skill in skills"
            :key="skill.id"
          >
            <b-form-checkbox
              v-model="pageFilter.ListSkillsId"
              :value="skill.id"
              >{{ skill.name }}</b-form-checkbox
            >
          </div>
        </div>
      </b-form-group>
    </b-collapse>

    <b-button
      button
      class="custom-btn primary d-block mt-2 mb-4"
      variant="primary"
      v-if="!hasEmployees && !isBusy && isAdmin"
      @click.prevent="handleCreateEmployee()"
    >
      <fa icon="plus" style="transform: scale(0.8)" /> Add
    </b-button>

    <div v-if="isBusy || hasEmployees" class="big-table-wrap table-responsive">
      <div class="mb-2 d-flex flex-wrap justify-content-between">
        <div class="d-flex">
          <b-button
            v-if="isAdmin"
            button
            @click.prevent="handleCreateEmployee()"
            class="custom-btn btn primary mr-2 mt-2"
            variant="primary"
            ><fa icon="plus" style="transform: scale(0.8)" /> Add</b-button
          >
          <h4
            v-show="totalRows !== null"
            style="color: #fff; margin-top: 13px; margin-left: 10px"
          >
            Found {{ totalRows }} employees
          </h4>
        </div>

        <b-button
          @click.prevent="handleDeleteSelectedEmployees()"
          v-if="selectedEmployees.length && isAdmin"
          class="custom-btn mt-2"
          variant="danger"
          ><fa icon="trash" style="transform: scale(0.8)" /> Delete
          selected</b-button
        >
      </div>

      <table
        :busy="isBusy"
        :class="
          isAdmin
            ? 'table table-hover table-striped table-borderless big-table big-table-buttons big-table-select-column '
            : 'table table-hover table-striped table-borderless big-table'
        "
      >
        <thead>
          <tr>
            <th class="col" v-if="isAdmin"></th>
            <th class="col-1" @click="orderBy('Id')">Id</th>
            <th class="col-1" @click="orderBy('LastName')">LastName</th>
            <th class="col-1" @click="orderBy('FirstName')">FirstName</th>
            <th class="col-1" @click="orderBy('MiddleName')">MiddleName</th>
            <th class="col-1" @click="orderBy('DepartmentName')">Department</th>
            <th class="col-1" @click="orderBy('Salary')">Salary</th>
            <th class="col-6" @click="orderBy('SkillsCount')">Skills</th>
            <th class="col" v-if="isAdmin"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="employee in employees" :key="employee.id">
            <td v-if="isAdmin">
              <b-form-checkbox
                v-model="selectedEmployees"
                :value="employee.id"
              ></b-form-checkbox>
            </td>
            <td>{{ employee.id }}</td>
            <td>{{ employee.lastName }}</td>
            <td>{{ employee.firstName }}</td>
            <td>{{ employee.middleName }}</td>
            <td>{{ employee.departmentName }}</td>
            <td>{{ employee.salary.toFixed(2) }} $</td>
            <td class="employee-skill-td">
              <span
                class="employee-skill-item"
                v-for="(skill, i) in employee.skills"
                :key="i"
              >
                {{ skill.name }}
              </span>
            </td>
            <td v-if="isAdmin">
              <div>
                <button @click.prevent="handleEditEmployee(employee)">
                  <fa class="mr-2 table-ico table-ico-primary" icon="edit" />
                </button>
                <button @click.prevent="handleDeleteEmployee(employee)">
                  <fa class="table-ico table-ico-danger" icon="trash" />
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-if="isBusy" class="text-center text-danger my-2">
        <b-spinner class="align-middle"></b-spinner>
        <strong>Loading...</strong>
      </div>
      <div class="pagination-wrap">
        <b-pagination
          v-show="pageFilter.take !== null"
          class="mt-2 custom-pagination"
          v-model="currentPage"
          :total-rows="totalRows"
          :per-page="pageFilter.take"
          @page-click="onPageClick"
        >
        </b-pagination>
        <div class="select-pagination-rows">
          <p>Rows on page</p>
          <b-form-select
            v-model="pageFilter.take"
            class="mb-3 take-pages-select"
          >
            <b-form-select-option :value="2">2</b-form-select-option>
            <b-form-select-option :value="5">5</b-form-select-option>
            <b-form-select-option :value="10">10</b-form-select-option>
            <b-form-select-option :value="100">100</b-form-select-option>
            <b-form-select-option :value="null">All</b-form-select-option>
          </b-form-select>
        </div>
      </div>
    </div>
    <span v-else>There are no employees</span>
  </section>
</template>

<script>
import DeleteEmployeeModal from "~/components/Employee/DeleteEmployeeModal.vue";
import EditEmployeeModal from "~/components/Employee/EditEmployeeModal.vue";
export default {
  components: { EditEmployeeModal, DeleteEmployeeModal },
  props: ["departmentId"],

  data() {
    return {
      currentPage: 1,
      totalRows: null,
      isBusy: true,
      isAdmin: false,
      selectedEmployees: [],
      employees: [],
      skills: [],
      departments: [],
      departmentParents: [],
      pageFilter: {
        take: 10,
        skip: 0,
        orderBy: "FirstName",
        ascDesc: "asc",
        departmentId: null,
        FirstName: "",
        minSalary: null,
        maxSalary: null,
        ListSkillsId: [],
      },
    };
  },

  watch: {
    "pageFilter.take"(newVal, oldVal) {
      if (newVal !== oldVal) {
        if (this.pageFilter.skip > newVal) {
          let skipped = Math.floor(this.pageFilter.skip / newVal);
          this.pageFilter.skip = skipped * newVal;
          this.currentPage = skipped + 1;
        } else {
          this.pageFilter.skip = 0;
          this.currentPage = 1;
        }
      }
      if (newVal === null) {
        this.pageFilter.skip = 0;
        this.currentPage = 1;
      }
    },
    pageFilter: {
      async handler(value) {
        if (value.maxSalary === "") {
          this.pageFilter.maxSalary = null;
        }
        if (value.minSalary === "") {
          this.pageFilter.minSalary = null;
        }
        this.employees = await this.getEmployees();
      },
      deep: true,
    },
  },

  computed: {
    hasEmployees() {
      return this.employees.length > 0;
    },
  },

  async mounted() {
    this.employees = await this.getEmployees();
    this.isAdmin = this.$auth.user.role === "Admin";
    this.isBusy = false;
    await this.$axios
      .get(`/api/department/childrenAndParents/childId/0`)
      .then((d) => {
        this.departments = d.data.children;
        this.departmentParents = d.data.parents;
      });
    await this.$axios.get("/api/skill").then((d) => {
      this.skills = d.data;
    });
  },

  methods: {
    async getEmployees() {
      const pageQuery = `?Take=${this.pageFilter.take}
                          &Skip=${this.pageFilter.skip}
                          &OrderBy=${this.pageFilter.orderBy}
                          &AscDesc=${this.pageFilter.ascDesc}`;
      const id = this.departmentId;
      let { data } =
        id === ""
          ? await this.$axios.post("/api/employee/filtred", this.pageFilter)
          : await this.$axios.get("/api/employee/department/" + id + pageQuery);

      this.totalRows = data.totalCount;

      return data.employees;
    },

    async setDepartmentFilter(department, index) {
      let { data } = await this.$axios.get(
        `/api/department/parentId/${department === null ? 0 : department.id}`
      );

      if (data.length > 0) {
        this.departments = data;
      }

      this.pageFilter.departmentId = department === null ? null : department.id;
      this.employees = await this.getEmployees();

      if (department === null && index === null) {
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
    },

    async onPageClick(event, page) {
      this.pageFilter.skip = (page - 1) * this.pageFilter.take;
    },

    async orderBy(orderBy) {
      if (this.pageFilter.orderBy === orderBy) {
        this.pageFilter.ascDesc =
          this.pageFilter.ascDesc === "asc" ? "desc" : "asc";
      } else {
        this.pageFilter.ascDesc = "asc";
      }
      this.pageFilter.orderBy = orderBy;
      this.pageFilter.skip = 0;
      this.currentPage = 1;
    },

    handleCreateEmployee() {
      this.$refs["editEmployeeModal"].open();
    },

    handleEditEmployee(employee) {
      this.$refs["editEmployeeModal"].open(employee);
    },

    handleDeleteEmployee(employee) {
      this.$refs["deleteEmployeeModal"].open(employee);
    },

    handleDeleteSelectedEmployees() {
      this.$refs["deleteEmployeeModal"].open(this.selectedEmployees);
    },

    async updateDataTable() {
      let response = await this.getEmployees();
      if (response.length === 0) {
        this.pageFilter.skip = 0;
        this.currentPage = 1;
        response = await this.getEmployees();
      }
      this.employees = response;
    },
  },
};
</script>


<style lang="scss" scoped>
.employee-skill-td {
  .employee-skill-item::after {
    content: ", ";
    position: relative;
    right: 2px;
  }
  .employee-skill-item:last-child::after {
    content: "";
  }
}
.filter-wrap {
  background-color: #d3d2d2;
  background-image: url('~/assets/images/anresh-logo.svg');
  background-repeat: no-repeat;
  background-size: 100px;
  background-position-x: 99%;
  background-position-y: 90%;
  background-size: auto 70%;
  padding: 0.5rem;
  border-radius: 7px;
  margin-bottom: 1rem;
  display: flex;
  .left-side-filter {
    width: 50%;
    display: block;
    .filter-inputs {
      display: flex;
      width: 100%;
      justify-content: space-between;
      .form-group {
        width: 32%;
        margin: 0;
      }
    }
    .department-filter {
      width: 100%;
      
      .select-department {
        scroll-snap-type: y mandatory;
        background-color: #fff;
        height: calc(38px * 4);
        li {
          line-height: 38px;
          scroll-snap-align: start;
        }
      }
    }
  }
  .select-skill-wrap {
    width: 100%;
    display: flex;
    justify-content: start;
    flex-wrap: wrap;
    margin-top: -5px;
    .select-skill-inner {
      margin: 5px;
      background-color: #fff;
      border-radius: 10px;
      padding: 0.39rem 0.7rem
    }
  }
}

.take-pages-select {
  height: 27px;
  padding: 0;
  width: 70px;
  background-color: #c8c7c7;
}

.pagination-wrap {
  width: 100%;
  display: flex;
  align-items: center;
  position: relative;
  height: 44px;
  .select-pagination-rows {
    position: absolute;
    right: 0;
    margin: 0.7rem 0;
    display: flex;
    height: 20px;
    align-items: normal;
    p {
      margin-right: 10px;
      color: #fff;
    }
  }
}
</style>